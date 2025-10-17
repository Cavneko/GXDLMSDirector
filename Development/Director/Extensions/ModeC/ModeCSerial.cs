// CUSTOM: Mode C Tool customization to ease future upstream merges.
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Director.Extensions.ModeC
{
    internal sealed class ModeCSerial : IDisposable
    {
        private SerialPort port;
        private readonly object portLock = new object();

        public ModeCSerial(string portName)
        {
            if (string.IsNullOrWhiteSpace(portName))
            {
                throw new ArgumentException("Port name must be provided.", nameof(portName));
            }

            PortName = portName;
        }

        public string PortName { get; }

        public int GuardMilliseconds { get; set; } = 80;

        public bool IsOpen
        {
            get
            {
                lock (portLock)
                {
                    return port?.IsOpen == true;
                }
            }
        }

        public void OpenInitial()
        {
            lock (portLock)
            {
                CloseInternal();
                port = new SerialPort(PortName, 300, Parity.Even, 7, StopBits.One)
                {
                    Handshake = Handshake.None,
                    DtrEnable = true,
                    RtsEnable = false,
                    ReadTimeout = 1200,
                    WriteTimeout = 1200,
                    Encoding = Encoding.ASCII,
                    NewLine = "\r\n"
                };
                port.Open();
            }
        }

        public void SwitchToHighSpeed()
        {
            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                p.BaudRate = 19200;
                p.Parity = Parity.Even;
                p.DataBits = 7;
                p.StopBits = StopBits.One;
                p.Handshake = Handshake.None;
                p.DtrEnable = true;
                p.RtsEnable = false;
            }
        }

        public void Write(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                p.Write(buffer, 0, buffer.Length);
            }
        }

        public byte[] ReadUntilCrLf(int timeoutMs)
        {
            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                int originalTimeout = p.ReadTimeout;
                p.ReadTimeout = timeoutMs;
                try
                {
                    List<byte> buffer = new List<byte>();
                    bool sawCr = false;
                    while (true)
                    {
                        int value = p.ReadByte();
                        if (value == -1)
                        {
                            continue;
                        }

                        byte b = (byte)value;
                        buffer.Add(b);
                        if (sawCr && b == 0x0A)
                        {
                            break;
                        }

                        sawCr = b == 0x0D;
                    }

                    return buffer.ToArray();
                }
                finally
                {
                    p.ReadTimeout = originalTimeout;
                }
            }
        }

        public byte[] ReadUntilEtxThenBcc(int timeoutMs)
        {
            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                int originalTimeout = p.ReadTimeout;
                p.ReadTimeout = timeoutMs;
                try
                {
                    List<byte> buffer = new List<byte>();
                    bool sawEtx = false;
                    while (true)
                    {
                        int value = p.ReadByte();
                        if (value == -1)
                        {
                            continue;
                        }

                        byte b = (byte)value;
                        buffer.Add(b);
                        if (sawEtx)
                        {
                            break;
                        }

                        if (b == 0x03)
                        {
                            sawEtx = true;
                        }
                    }

                    return buffer.ToArray();
                }
                finally
                {
                    p.ReadTimeout = originalTimeout;
                }
            }
        }

        public byte[] ReadAvailable(int timeoutMs)
        {
            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                List<byte> buffer = new List<byte>();
                int originalTimeout = p.ReadTimeout;
                p.ReadTimeout = Math.Max(1, Math.Min(timeoutMs, 100));
                try
                {
                    int remaining = timeoutMs;
                    while (remaining > 0)
                    {
                        int started = Environment.TickCount;
                        try
                        {
                            int value = p.ReadByte();
                            if (value == -1)
                            {
                                continue;
                            }
                            buffer.Add((byte)value);
                        }
                        catch (TimeoutException)
                        {
                            break;
                        }
                        finally
                        {
                            int elapsed = Environment.TickCount - started;
                            if (elapsed < 0)
                            {
                                elapsed = 0;
                            }
                            remaining -= Math.Max(1, elapsed);
                        }
                    }

                    return buffer.ToArray();
                }
                finally
                {
                    p.ReadTimeout = originalTimeout;
                }
            }
        }

        public int ReadByteWithTimeout(int timeoutMs)
        {
            lock (portLock)
            {
                EnsureOpen();
                SerialPort p = port;
                if (p == null)
                {
                    throw new InvalidOperationException("Serial port is not open.");
                }
                int originalTimeout = p.ReadTimeout;
                p.ReadTimeout = timeoutMs;
                try
                {
                    return p.ReadByte();
                }
                finally
                {
                    p.ReadTimeout = originalTimeout;
                }
            }
        }

        public void Close()
        {
            lock (portLock)
            {
                CloseInternal();
            }
        }

        private void CloseInternal()
        {
            if (port != null)
            {
                try
                {
                    if (port.IsOpen)
                    {
                        port.Close();
                    }
                }
                finally
                {
                    port.Dispose();
                    port = null;
                }
            }
        }

        private void EnsureOpen()
        {
            if (port == null || !port.IsOpen)
            {
                throw new InvalidOperationException("Serial port is not open.");
            }
        }

        public void Dispose()
        {
            Close();
        }

        public static byte[] BuildP1(string password)
        {
            if (password == null)
            {
                password = string.Empty;
            }
            byte[] payload = Encoding.ASCII.GetBytes($"({password})");
            byte[] frame = new byte[1 + 2 + 1 + payload.Length + 1 + 1];
            int index = 0;
            frame[index++] = 0x01;
            frame[index++] = (byte)'P';
            frame[index++] = (byte)'1';
            frame[index++] = 0x02;
            Buffer.BlockCopy(payload, 0, frame, index, payload.Length);
            index += payload.Length;
            frame[index++] = 0x03;
            byte bcc = ComputeBcc(new ReadOnlySpan<byte>(frame, 1, index - 1));
            frame[index] = bcc;
            return frame;
        }

        public static byte[] BuildR1(string obis)
        {
            byte[] payload = Encoding.ASCII.GetBytes($"{obis}()");
            byte[] frame = new byte[1 + 2 + 1 + payload.Length + 1 + 1];
            int index = 0;
            frame[index++] = 0x01;
            frame[index++] = (byte)'R';
            frame[index++] = (byte)'1';
            frame[index++] = 0x02;
            Buffer.BlockCopy(payload, 0, frame, index, payload.Length);
            index += payload.Length;
            frame[index++] = 0x03;
            byte bcc = ComputeBcc(new ReadOnlySpan<byte>(frame, 1, index - 1));
            frame[index] = bcc;
            return frame;
        }
        public static byte[] BuildW1(string obis, string data)
        {
            byte[] payload = Encoding.ASCII.GetBytes($"{obis}({data})");
            byte[] frame = new byte[1 + 2 + 1 + payload.Length + 1 + 1];
            int index = 0;
            frame[index++] = 0x01;
            frame[index++] = (byte)'W';
            frame[index++] = (byte)'1';
            frame[index++] = 0x02;
            Buffer.BlockCopy(payload, 0, frame, index, payload.Length);
            index += payload.Length;
            frame[index++] = 0x03;
            byte bcc = ComputeBcc(new ReadOnlySpan<byte>(frame, 1, index - 1));
            frame[index] = bcc;
            return frame;
        }

        public static byte[] BuildB0()
        {
            byte[] frame = new byte[5];
            frame[0] = 0x01;
            frame[1] = (byte)'B';
            frame[2] = (byte)'0';
            frame[3] = 0x03;
            frame[4] = ComputeBcc(new ReadOnlySpan<byte>(frame, 1, 3));
            return frame;
        }

        public static byte ComputeBcc(ReadOnlySpan<byte> data)
        {
            byte result = 0;
            for (int i = 0; i < data.Length; ++i)
            {
                result ^= data[i];
            }
            return result;
        }
    }
}
