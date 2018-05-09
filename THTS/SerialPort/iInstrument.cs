using System.Text;
using System.IO.Ports;
using SystemEx.Protocols.Cppi;

namespace THTS.SerialPort
{
    unsafe public class iInstrument : iSerialPort
    {
        public iInstrument(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
        }

        /// <summary>
        /// 获取设备类型
        /// </summary>
        /// <returns></returns>
        public string GetDeviceType()
        {
            #region Test
            return "DPC";
            #endregion

            try
            {
                CTelegram telegram = new CTelegram(0x00, 500);
                CDatagram datagram = this.SendTelegram(telegram, 3);

                StringBuilder sb = new StringBuilder();
                fixed (byte* pFile = datagram.Block)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (pFile[i] == 0)
                        {
                            break;
                        }
                        sb.Append((char)pFile[i]);
                    }
                }

                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设备是否在线
        /// </summary>
        /// <returns></returns>
        public bool IsOnline()
        {
            return !GetDeviceType().Equals(string.Empty);
        }

        /// <summary>
        /// 获取传感器状态
        /// </summary>
        /// <returns></returns>
        public bool GetSensorState(out ChannelState sensorState)
        {
            try
            {
                ChannelState state = new ChannelState();

                #region 测试内容
                ChannelEachState sensor1 = new ChannelEachState();
                sensor1.ChannelType = "DPC.Temperature";
                sensor1.IsOnline = true;
                ChannelEachState sensor2 = new ChannelEachState();
                sensor2.ChannelType = "DPC.Temperature";
                sensor2.IsOnline = false;
                ChannelEachState sensor3 = new ChannelEachState();
                sensor3.ChannelType = "DPC.Humidity";
                sensor3.IsOnline = true;
                ChannelEachState sensor4 = new ChannelEachState();
                sensor4.ChannelType = "DPC.SampleBoard";
                sensor4.IsOnline = true;

                state.Channel1 = sensor1;
                state.Channel2 = sensor2;
                state.Channel3 = sensor3;
                state.Channel4 = sensor4;

                sensorState = state;
                return true;
                #endregion

                //读取任务基本信息
                CTelegram telegram = new CTelegram(0x00, 501);
                CDatagram datagram = this.SendTelegram(telegram, 3);
                fixed (byte* pBuf = datagram.Block)
                {
                    Win32.memcpy(&state, pBuf, datagram.DataLength);
                }
                sensorState = state;
            }
            catch
            {
                sensorState = new ChannelState();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取传感器数据
        /// </summary>
        /// <returns></returns>
        public bool GetSensorValue(out ChannelValue sensorValue)
        {
            try
            {
                ChannelValue measureValue = new ChannelValue();

                #region 测试内容
                return true;
                #endregion

                //读取任务基本信息
                CTelegram telegram = new CTelegram(0x00, 502);
                CDatagram datagram = this.SendTelegram(telegram, 3);
                fixed (byte* pBuf = datagram.Block)
                {
                    Win32.memcpy(&measureValue, pBuf, datagram.DataLength);
                }
                sensorValue = measureValue;
            }
            catch
            {
                sensorValue = new ChannelValue();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取全部传感器实时数据
        /// </summary>
        /// <param name="allSensorValue"></param>
        /// <returns></returns>
        public bool GetALLSensorValue(out ALLSensorValue allSensorValue)
        {
            #region Test
            return true;
            #endregion

            try
            {
                ALLSensorValue measureValue = new ALLSensorValue();

                //读取任务基本信息
                CTelegram telegram = new CTelegram(0x00, 502);
                CDatagram datagram = this.SendTelegram(telegram, 3);
                fixed (byte* pBuf = datagram.Block)
                {
                    Win32.memcpy(&measureValue, pBuf, datagram.DataLength);
                }
                allSensorValue = measureValue;
            }
            catch
            {
                allSensorValue = new ALLSensorValue();
                return false;
            }

            return true;
        }
    }
}
