using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.CodeDom.Compiler;

namespace Director.Extensions.ModeC
{
    public partial class ModeCTool : UserControl
    {
        // CUSTOM: Mode C Tool customization to ease future upstream merges.
        private readonly object serialGate = new object();
        private ModeCSerial serial;
        private int operationRunning;
        private List<ObisEntry> obisEntries;
        private HashSet<string> expandedCategories = new HashSet<string>();

        public ModeCTool()
        {
            InitializeComponent();
        }

        private void ModeCTool_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                RefreshPorts();
                UpdateControlState();
                LoadObis();
                PopulateObisTree();
            }
        }

        private void cmbPort_DropDown(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private void RefreshPorts()
        {
            string selected = cmbPort.SelectedItem as string;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                Array.Sort(ports, StringComparer.OrdinalIgnoreCase);
                cmbPort.BeginUpdate();
                cmbPort.Items.Clear();
                cmbPort.Items.AddRange(ports);
                if (!string.IsNullOrEmpty(selected))
                {
                    int idx = Array.IndexOf(ports, selected);
                    if (idx >= 0)
                    {
                        cmbPort.SelectedIndex = idx;
                    }
                    else if (ports.Length > 0)
                    {
                        cmbPort.SelectedIndex = 0;
                    }
                }
                else if (ports.Length > 0)
                {
                    cmbPort.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                cmbPort.EndUpdate();
            }
        }

        private bool BeginOperation()
        {
            if (Interlocked.CompareExchange(ref operationRunning, 1, 0) != 0)
            {
                return false;
            }
            UpdateControlState();
            return true;
        }

        private void EndOperation()
        {
            Interlocked.Exchange(ref operationRunning, 0);
            UpdateControlState();
        }

        private Task RunOperationAsync(Action action)
        {
            if (!BeginOperation())
            {
                return Task.CompletedTask;
            }

            return Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
                finally
                {
                    EndOperation();
                }
            });
        }

        private void UpdateControlState()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateControlState));
                return;
            }

            bool busy = Volatile.Read(ref operationRunning) != 0;
            btnRun.Enabled = !busy;
            cmbPort.Enabled = !busy;
            numGuard.Enabled = !busy;
            txtPassword.Enabled = !busy;
        }

        private void PerformCommand(string portName, int guardMilliseconds, string password, string mode, List<ObisEntry> CheckedEntries, string data)
        {
            ModeCSerial previous = DetachCurrentSerial();
            if (previous != null)
            {
                previous.Dispose();
            }
            SetResultText(null);

            ModeCSerial newSerial = null;
            bool sessionReady = false;
            bool closeSent = false;
            Dictionary<string, string> results = new Dictionary<string, string>();

            try
            {
                newSerial = new ModeCSerial(portName)
                {
                    GuardMilliseconds = guardMilliseconds
                };
                lock (serialGate)
                {
                    serial = newSerial;
                }
                newSerial.OpenInitial();

                byte[] cmd1 = Encoding.ASCII.GetBytes("/?!\r\n");
                LogFrame("CMD1", "TX", cmd1);
                newSerial.Write(cmd1);

                byte[] ident = newSerial.ReadUntilCrLf(1200);
                LogFrame("IDENT", "RX", ident);

                byte[] cmd2 = new byte[] { 0x06, (byte)'0', (byte)'6', (byte)'1', 0x0D, 0x0A };
                LogFrame("CMD2", "TX", cmd2);
                newSerial.Write(cmd2);

                Thread.Sleep(Math.Max(0, guardMilliseconds));

                newSerial.SwitchToHighSpeed();
                Log("[SWITCH] 19200 7E1 DTR=1 RTS=0");

                Thread.Sleep(100);

                byte[] p0Response = null;
                try
                {
                    p0Response = newSerial.ReadUntilEtxThenBcc(1200);
                    LogFrame("P0", "RX", p0Response);
                }
                catch (TimeoutException)
                {
                    Log("[ERR] Timeout waiting response for P0.");
                }

                if (p0Response == null || p0Response.Length < 2)
                {
                    Log("[ERR] P0 response too short.");
                    return;
                }

                byte p0Bcc = p0Response[p0Response.Length - 1];
                byte[] p0WithoutBcc = new byte[p0Response.Length - 1];
                Buffer.BlockCopy(p0Response, 0, p0WithoutBcc, 0, p0WithoutBcc.Length);
                byte expectedP0Bcc = ModeCSerial.ComputeBcc(new ReadOnlySpan<byte>(p0Response, 1, p0Response.Length - 2));
                if (expectedP0Bcc != p0Bcc)
                {
                    Log($"[ERR] P0 BCC mismatch. Expected {expectedP0Bcc:X2}, received {p0Bcc:X2}.");
                }

                int p0StxIndex = Array.IndexOf(p0WithoutBcc, (byte)0x02);
                int p0EtxIndex = Array.IndexOf(p0WithoutBcc, (byte)0x03);
                if (p0StxIndex < 0 || p0EtxIndex <= p0StxIndex)
                {
                    Log("[ERR] Unexpected P0 response format.");
                    return;
                }

                sessionReady = true;
                Log("[INFO] Mode C session ready.");

                byte[] p1Frame = ModeCSerial.BuildP1(password);
                LogFrame("P1", "TX", p1Frame);
                newSerial.Write(p1Frame);
                try
                {
                    int value = newSerial.ReadByteWithTimeout(1200);
                    if (value == 0x06)
                    {
                        Log("[P1] RX: ACK");
                    }
                    else if (value >= 0)
                    {
                        LogFrame("P1", "RX", new[] { (byte)value });
                    }
                }
                catch (TimeoutException)
                {
                    Log("[ERR] Timeout waiting ACK for P1.");
                }

                byte[] frame = null;
                string obis;
                foreach (ObisEntry obisEntry in CheckedEntries)
                {
                    obis = obisEntry.Obis;
                    switch (mode) 
                    {
                        case "Write":
                            frame = ModeCSerial.BuildW1(obis, data);
                            LogFrame("W1", "TX", frame);
                            newSerial.Write(frame);
                            break;
                        case "Read":
                            frame = ModeCSerial.BuildR1(obis);
                            LogFrame("R1", "TX", frame);
                            newSerial.Write(frame);
                            break;
                        case "Execute":
                            frame = ModeCSerial.BuildE2(obis, data);
                            LogFrame("E2", "TX", frame);
                            newSerial.Write(frame);
                            break;
                        default: 
                            break;
                    }

                    byte[] response = null;
                    try
                    {
                        switch (mode)
                        {
                            case "Write":
                                int writeValue = newSerial.ReadByteWithTimeout(1200);
                                if (writeValue == 0x06)
                                {
                                    Log("[W1] RX: ACK");
                                    results.Add(obisEntry.Obis, "ACK");
                                }
                                else if (writeValue == 0x15)
                                {
                                    Log("[W1] RX: NAK");
                                    results.Add(obisEntry.Obis, "NAK");
                                }
                                else if (writeValue > 0)
                                {
                                    LogFrame("W1", "RX", new[] { (byte)writeValue });
                                }
                                break;
                            case "Read":
                                response = newSerial.ReadUntilEtxThenBcc(1200);
                                LogFrame("R1", "RX", response);
                                break;
                            case "Execute":
                                int executeValue = newSerial.ReadByteWithTimeout(1200);
                                if (executeValue == 0x06)
                                {
                                    Log("E2 RX: ACK");
                                    results.Add(obisEntry.Obis, "ACK");
                                }
                                else if (executeValue == 0x15)
                                {
                                    Log("E2: RX: NAK");
                                    results.Add(obisEntry.Obis, "NAK");
                                }
                                else if (executeValue >= 0)
                                {
                                    LogFrame("E2", "RX", new[] { (byte)executeValue });
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (TimeoutException)
                    {
                        switch (mode)
                        {
                            case "Write":
                                Log("[ERR] Timeout waiting for W1 response.");
                                break;
                            case "Read":
                                Log("[ERR] Timeout waiting for R1 response.");
                                break;
                            case "Execute":
                                Log("[ERR] Timeout waitinf for E2 response.");
                                break;
                            default:
                                break;
                        }
                    }

                    if (response != null)
                    {
                        if (response.Length < 2)
                        {
                            Log("[ERR] Response too short.");
                        }
                        else
                        {
                            byte receivedBcc = response[response.Length - 1];
                            byte[] payload = new byte[response.Length - 1];
                            Buffer.BlockCopy(response, 0, payload, 0, payload.Length);

                            int stxIndex = Array.IndexOf(payload, (byte)0x02);
                            int etxIndex = Array.IndexOf(payload, (byte)0x03);
                            if (stxIndex < 0 || etxIndex <= stxIndex)
                            {
                                Log("[ERR] Unexpected response format.");
                            }
                            else
                            {
                                byte expectedBcc = ModeCSerial.ComputeBcc(new ReadOnlySpan<byte>(payload, stxIndex + 1, etxIndex - stxIndex));
                                if (receivedBcc != expectedBcc)
                                {
                                    Log($"[ERR] BCC mismatch. Expected {expectedBcc:X2}, received {receivedBcc:X2}.");
                                }

                                string ascii = Encoding.ASCII.GetString(payload, stxIndex + 1, etxIndex - stxIndex - 1);
                                string value = ExtractValue(ascii);
                                if (value != null)
                                {
                                    results.Add(obisEntry.Obis, value);
                                }
                                else
                                {
                                    Log($"[ERR] Cannot parse value from '{ascii}'.");
                                }
                            }
                        }
                    }
                }

                SetResultText(results);

                if (sessionReady && newSerial.IsOpen)
                {
                    byte[] closeFrame = ModeCSerial.BuildB0();
                    LogFrame("B0", "TX", closeFrame);
                    newSerial.Write(closeFrame);
                    closeSent = true;
                }
            }
            catch (TimeoutException ex)
            {
                Log($"[ERR] Timeout: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Log($"[ERR] Access denied: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                if (newSerial != null)
                {
                    try
                    {
                        if (/*sessionReady &&*/ !closeSent && newSerial.IsOpen)
                        {
                            byte[] closeFrame = ModeCSerial.BuildB0();
                            LogFrame("B0", "TX", closeFrame);
                            newSerial.Write(closeFrame);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"[WARN] Failed to send B0 close: {ex.Message}");
                    }

                    try
                    {
                        newSerial.Close();
                        Log("[INFO] Port closed.");
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                    }

                    newSerial.Dispose();
                    lock (serialGate)
                    {
                        if (ReferenceEquals(serial, newSerial))
                        {
                            serial = null;
                        }
                    }
                }
            }
        }

        private ModeCSerial DetachCurrentSerial()
        {
            lock (serialGate)
            {
                ModeCSerial current = serial;
                serial = null;
                return current;
            }
        }

        private static string ExtractValue(string ascii)
        {
            if (string.IsNullOrEmpty(ascii))
            {
                return null;
            }

            int start = ascii.IndexOf('(');
            if (start < 0)
            {
                return null;
            }
            int end = ascii.IndexOf(')', start + 1);
            if (end < 0 || end <= start + 1)
            {
                return null;
            }
            return ascii.Substring(start + 1, end - start - 1);
        }

        private void SetResultText(Dictionary<string, string> results)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<Dictionary<string, string>>(SetResultText), results);
                return;
            }

            if (results == null || results.Count == 0)
            {
                txtResult.Text = string.Empty;
                return;
            }

            var sb = new StringBuilder();
            foreach (var kvp in results)
            {
                sb.AppendLine($"{kvp.Key}: {kvp.Value}");
            }

            txtResult.Text = sb.ToString();
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(Log), message);
                return;
            }

            if (txtLog.TextLength > 0)
            {
                txtLog.AppendText(Environment.NewLine);
            }
            txtLog.AppendText($"{DateTime.Now:HH:mm:ss.fff} {message}");
        }

        private void LogError(Exception ex)
        {
            Log($"[ERR] {ex.Message}");
        }

        private void LogFrame(string stage, string direction, byte[] data)
        {
            Log($"[{stage}] {direction}: {HX(data)} ascii='{FormatAscii(data)}'");
        }

        public static string HX(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return "(empty)";
            }

            StringBuilder sb = new StringBuilder(data.Length * 3);
            for (int i = 0; i < data.Length; ++i)
            {
                if (i > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(data[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static string FormatAscii(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(data.Length);
            foreach (byte b in data)
            {
                switch (b)
                {
                    case 0x0D:
                        sb.Append("\\r");
                        break;
                    case 0x0A:
                        sb.Append("\\n");
                        break;
                    default:
                        if (b >= 0x20 && b <= 0x7E)
                        {
                            sb.Append((char)b);
                        }
                        else
                        {
                            sb.Append('.');
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        private void txtObis_TextChanged(object sender, EventArgs e)
        {
            string mode = cmbMode.SelectedItem as string;
            string filter = txtObis.Text;
            PopulateObisTree(mode, filter);
        }
        
        private void LoadObis()
        {
            var assembly = Assembly.GetExecutingAssembly();
            //string resourceName = "GXDLMSDdirector46.Director.Extensions.ModeC.obis_list.json";
            string resourceName = "GXDLMSDirector.Director.Extensions.ModeC.obis_list.json";

            using (Stream stream = assembly .GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonText = reader.ReadToEnd();
                obisEntries = JsonConvert.DeserializeObject<List<ObisEntry>>(jsonText);
            }

        }

        private void PopulateObisTree(string mode = "All", string filter = null)
        {
            treeObis.Nodes.Clear();

            var grouped = obisEntries.GroupBy(o => o.Category);

            foreach (var group in grouped)
            {
                TreeNode categoryNode = new TreeNode(group.Key);
                IEnumerable<ObisEntry> entries = group;
                switch (mode)
                {
                    case "Read":
                        entries = group.Where(o => o.CanRead);
                        break;
                    case "Write":
                        entries = group.Where(o => o.CanWrite);
                        break;
                    case "Execute":
                        entries = group.Where(o => o.CanExecute);
                        break;
                    case "All":
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(filter))
                {
                    string lowerFilter = filter.ToLower();
                    entries = entries.Where(o => 
                        o.Name.ToLower().Contains(lowerFilter) ||
                        o.Obis.ToLower().Contains(lowerFilter) ||
                        o.Category.ToLower().Contains(lowerFilter)
                    );
                }

                foreach (var entry in entries)
                {
                    TreeNode node = new TreeNode($"{entry.Name} [{entry.Obis}]");
                    node.Tag = entry;
                    categoryNode.Nodes.Add(node);
                }

                if (categoryNode.Nodes.Count > 0)
                {
                    treeObis.Nodes.Add(categoryNode);
                }
            }
            int totalNodes = treeObis.Nodes.Count;
            foreach (TreeNode node in treeObis.Nodes)
            {
                totalNodes += node.GetNodeCount(true);
            }
           if (totalNodes < 15)
            {
                treeObis.ExpandAll();
            }
            else
            {
                foreach (TreeNode node in treeObis.Nodes)
                {
                    if (expandedCategories.Contains(node.Text))
                    {
                        node.Expand();
                    }
                }
            }
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMode = cmbMode.SelectedItem?.ToString();

            switch (selectedMode)
            {
                case "Read":
                    PopulateObisTree("Read", txtObis.Text);
                    break;
                case "Write":
                    PopulateObisTree("Write", txtObis.Text);
                    break;
                case "Execute":
                    PopulateObisTree("Execute", txtObis.Text);
                    break;
                default:
                    PopulateObisTree("All");
                    break;
            }
        }

        private void treeObis_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            expandedCategories.Add(e.Node.Text);
        }

        private void treeObis_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            expandedCategories.Remove(e.Node.Text);
        }

        private List<ObisEntry> GetCheckedObisEntry()
        {
            List<ObisEntry> checkedEntires = new List<ObisEntry>();

            foreach (TreeNode category in treeObis.Nodes)
            {
                foreach (TreeNode leaf in category.Nodes)
                {
                    if (leaf.Checked && leaf.Tag is ObisEntry entry)
                    {
                        checkedEntires.Add(entry);
                    }
                }
            }
            return checkedEntires;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            string port = cmbPort.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(port))
            {
                Log("[ERR] Select a serial port first.");
                return;
            }

            if (GetCheckedObisEntry().Count == 0)
            {
                Log("[ERR] Select an obis code first.");
                return;
            }

            string mode = cmbMode.Text;
            if (string.IsNullOrEmpty(mode))
            {
                Log("[ERR] Select an action mode first.");
                return;
            }

            int guard = (int)numGuard.Value;
            string password = txtPassword.Text ?? string.Empty;
            List<ObisEntry> CheckedEntries = GetCheckedObisEntry();
            string data = txtData.Text;
            await RunOperationAsync(() => PerformCommand(port, guard, password, mode, CheckedEntries, data));
        }

        private void treeObis_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null)
            {
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = e.Node.Checked;
                }
            }
        }
    }
}
