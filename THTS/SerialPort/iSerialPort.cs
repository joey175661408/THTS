using System;
using System.Collections.Generic;
using System.Text;

namespace THTS.SerialPort
{
    using System.IO.Ports;
    using SystemEx.Protocols.Cppi;

    public class iSerialPort : SerialPort
    {
        string commandAppendix = "\0";
        /// <summary>
        /// Command's Appendix
        /// </summary>
        public string CommandAppendix
        {
            get { return commandAppendix; }
            set { commandAppendix = value; }
        }
        string[] discardString = new string[] { "\0", "\r", "\n" };
        /// <summary>
        /// 
        /// </summary>
        public string[] DiscardString
        {
            get { return discardString; }
            set { discardString = value; }
        }
        int readingTimeout = 100;
        /// <summary>
        /// Timeout per readexisting(milliseconds)
        /// </summary>
        public int ReadingTimeout
        {
            get { return readingTimeout; }
            set { readingTimeout = value; }
        }
        public iSerialPort()
            : base("COM1", 9600, Parity.None, 8, StopBits.One)
        {
            this.Encoding = Encoding.GetEncoding("iso-8859-1");   //支持无称号ASCII.
        }
        public iSerialPort(string portName)
            : base(portName, 9600, Parity.None, 8, StopBits.One)
        {
            this.Encoding = Encoding.GetEncoding("iso-8859-1");   //支持无称号ASCII.
        }
        public iSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
            this.Encoding = Encoding.GetEncoding("iso-8859-1");   //支持无称号ASCII.
        }
        public new void Close()
        {
            try
            {
                if (base.IsOpen)
                    base.Close();
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// Open
        /// </summary>
        /// <returns></returns>
        public new bool Open()
        {
            try
            {
                if (!base.IsOpen)
                    base.Open();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Sleep
        /// </summary>
        /// <param name="milliSeconds">MilliSeconds</param>
        public static void Sleep(int milliSeconds)
        {
            DateTime start = DateTime.Now;
            while (true)
            {
                System.Threading.Thread.Sleep(20);
                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalMilliseconds >= milliSeconds)
                    break;
            }
        }
        /// <summary>
        /// Send Command & Receive Existing Count.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="breakFlag"></param>
        /// <param name="emptyOutCount"></param>
        /// <returns></returns>
        public string SendCommand(string command, string breakFlag, int emptyOutCount)
        {
            string received = string.Empty;
            if (!this.IsOpen)
                return string.Empty;
            this.DiscardInBuffer();
            this.DiscardOutBuffer();
            this.Write(string.Format("{0}{1}", command, this.CommandAppendix));
            //
            int emptyCounter = 0;
            while (true)
            {
                Sleep(this.ReadingTimeout);
                string existing = this.ReadExisting();
                if (existing == string.Empty)
                {
                    if ((++emptyCounter) >= emptyOutCount)
                        break;
                }
                else
                {
                    emptyCounter = 0;
                    received += existing;
                }
                if (received.Contains(breakFlag))
                {
                    break;
                }
            }
            if (this.DiscardString != null && this.DiscardString.Length > 0)
            {
                foreach (string discards in this.DiscardString)
                {
                    if (discards != string.Empty)
                        received = received.Replace(discards, string.Empty);
                }
            }
            return received;
        }
        public virtual byte[] SendTelegram(string command, int emptyOutCount)
        {
            lock (this)
            {
                List<byte> data = new List<byte>();
                if (!this.IsOpen)
                    return null;
                this.DiscardInBuffer();
                this.Write(string.Format("{0}{1}", command, this.CommandAppendix));
                //
                int emptyCounter = 0;
                while (true)
                {
                    Sleep(this.ReadingTimeout);
                    if (this.BytesToRead > 0)
                    {
                        while (this.BytesToRead > 0)
                        {
                            data.Add((byte)this.ReadByte());
                        }
                        emptyCounter = 0;
                    }
                    else
                    {
                        if ((++emptyCounter) >= emptyOutCount)
                            break;
                    }
                    int dataCount = data.Count;
                    if (dataCount >= 4 && (data[dataCount - 1] == 0xaa && data[dataCount - 2] == 0xbb && data[dataCount - 3] == 0xcc && data[dataCount - 4] == 0xdd))
                    {
                        break;
                    }
                }
                return data.ToArray();
            }
        }
        public string SendCommand(string command, int emptyOutCount)
        {
            string received = string.Empty;
            if (!this.IsOpen)
                return string.Empty;
            this.DiscardInBuffer();
            this.Write(string.Format("{0}{1}", command, this.CommandAppendix));
            Console.WriteLine("{0}", command);
            int emptyCounter = 0;
            while (true)
            {
                Sleep(this.ReadingTimeout);
                string existing = this.ReadExisting();
                if (existing == string.Empty)
                {
                    if ((++emptyCounter) >= emptyOutCount)
                        break;
                }
                else
                {
                    emptyCounter = 0;
                    received += existing;
                }
            }
            return received;
        }
        public string SendCommand(byte[] command, string breakFlag, int emptyOutCount)
        {
            string received = string.Empty;
            if (!this.IsOpen)
                return string.Empty;
            this.DiscardInBuffer();
            this.Write(string.Format("{0}{1}", command, this.CommandAppendix));
            //
            int emptyCounter = 0;
            while (true)
            {
                Sleep(this.ReadingTimeout);
                string existing = this.ReadExisting();
                if (existing == string.Empty)
                {
                    if ((++emptyCounter) >= emptyOutCount)
                        break;
                }
                else
                {
                    emptyCounter = 0;
                    received += existing;
                }
                if (received.Contains(breakFlag))
                {
                    break;
                }
            }
            if (this.DiscardString != null && this.DiscardString.Length > 0)
            {
                foreach (string discards in this.DiscardString)
                {
                    if (discards != string.Empty)
                        received = received.Replace(discards, string.Empty);
                }
            }
            return received;
        }
        /// <summary>
        /// 发送CPPI指令，并接收CPPI结果
        /// </summary>
        /// <param name="message"></param>
        /// <param name="emptyOutCount"></param>
        /// <returns></returns>
        public CDatagram SendTelegram(CTelegram message, uint emptyOutCount)
        {
            lock (this)
            {
                List<byte> data = new List<byte>();
                if (!this.IsOpen)
                    return new CDatagram();
                this.DiscardInBuffer();
                this.Write(message.Telegram, 0, message.Telegram.Length);
                //
                int emptyCounter = 0;
                while (true)
                {
                    //Sleep(this.PollingTime);
                    System.Threading.Thread.Sleep(20);
                    int bytesToRead = this.BytesToRead;
                    if (bytesToRead > 0)
                    {
                        byte[] bytes = new byte[bytesToRead];
                        this.Read(bytes, 0, bytesToRead);
                        data.AddRange(bytes);
                        emptyCounter = 0;
                    }
                    else
                    {
                        if ((++emptyCounter) >= emptyOutCount)
                            break;
                    }
                    int dataCount = data.Count;
                    if (dataCount >= 4 && (data[dataCount - 1] == 0xaa && data[dataCount - 2] == 0xbb && data[dataCount - 3] == 0xcc && data[dataCount - 4] == 0xdd))
                    {
                        break;
                    }
                }
                return new CDatagram(data.ToArray());
            }
        }
    }
}
