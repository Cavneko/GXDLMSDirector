using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Director.Extensions.ModeC
{
    public partial class ModeCTool : UserControl
    {
        // CUSTOM: Mode C Tool customization to ease future upstream merges.
        private readonly object serialGate = new object();
        private ModeCSerial serial;
        private int operationRunning;

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
            }
        }

        private void cmbPort_DropDown(object sender, EventArgs e)
        {
            RefreshPorts();
        }

        private async void btnRead_Click(object sender, EventArgs e)
        {
            string port = cmbPort.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(port))
            {
                Log("[ERR] Select a serial port first.");
                return;
            }

            int guard = (int)numGuard.Value;
            string password = txtPassword.Text ?? string.Empty;
            await RunOperationAsync(() => PerformRead180(port, guard, password));
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
            btnRead.Enabled = !busy;
            cmbPort.Enabled = !busy;
            numGuard.Enabled = !busy;
            txtPassword.Enabled = !busy;
        }

        private void PerformRead180(string portName, int guardMilliseconds, string password)
        {
            ModeCSerial previous = DetachCurrentSerial();
            if (previous != null)
            {
                previous.Dispose();
            }
            SetResultText(string.Empty);

            ModeCSerial newSerial = null;
            bool sessionReady = false;
            bool closeSent = false;
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

                Thread.Sleep(10);

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

                byte[] frame = ModeCSerial.BuildR1_180();
                LogFrame("R1", "TX", frame);
                newSerial.Write(frame);

                byte[] response = null;
                try
                {
                    response = newSerial.ReadUntilEtxThenBcc(1200);
                    LogFrame("R1", "RX", response);
                }
                catch (TimeoutException)
                {
                    Log("[ERR] Timeout waiting response for R1.");
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
                            byte expectedBcc = ModeCSerial.ComputeBcc(new ReadOnlySpan<byte>(payload, stxIndex, etxIndex - stxIndex + 1));
                            if (receivedBcc != expectedBcc)
                            {
                                Log($"[ERR] BCC mismatch. Expected {expectedBcc:X2}, received {receivedBcc:X2}.");
                            }

                            string ascii = Encoding.ASCII.GetString(payload, stxIndex + 1, etxIndex - stxIndex - 1);
                            string value = ExtractValue(ascii);
                            if (value != null)
                            {
                                SetResultText(value);
                            }
                            else
                            {
                                Log($"[ERR] Cannot parse value from '{ascii}'.");
                            }
                        }
                    }
                }

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
                        if (sessionReady && !closeSent && newSerial.IsOpen)
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

        private void SetResultText(string value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(SetResultText), value);
                return;
            }
            txtResult.Text = value ?? string.Empty;
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
    }
}
