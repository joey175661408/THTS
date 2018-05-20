using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Collections.ObjectModel;

namespace THTS.SerialPort
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ChannelValue
    {
        #region 结构体数据字段
        //共240byte，每60byte为一个从机的全部传感器报文
        fixed byte _sensorValue1[10 * 6];
        fixed byte _sensorValue2[10 * 6];
        fixed byte _sensorValue3[10 * 6];
        fixed byte _sensorValue4[10 * 6];

        #endregion

        #region 属性
        /// <summary>
        /// 1号通道传感器
        /// </summary>
        public ChannelEachValue[] Channel1
        {
            get
            {
                ChannelEachValue[] records = new ChannelEachValue[10];

                byte[] buffer = new byte[60];
                fixed (byte* pbuf = buffer, rbuf = _sensorValue1)
                {
                    Win32.memcpy(pbuf, rbuf, buffer.Length * sizeof(byte));
                }

                for (int index = 0; index < 60; index += 6)
                {
                    byte[] temp = new byte[6];
                    for (int x = 0; x < temp.Length; x++)
                    {
                        temp[x] = buffer[index + x];
                    }

                    ChannelEachValue runrecord = new ChannelEachValue();
                    fixed (byte* pbuf = temp)
                    {
                        Win32.memcpy(&runrecord, pbuf, sizeof(ChannelEachValue));
                    }
                    records[index / 6] = runrecord;
                }
                return records;
            }
        }
        /// <summary>
        /// 2号通道传感器
        /// </summary>
        public ChannelEachValue[] Channel2
        {
            get
            {
                ChannelEachValue[] records = new ChannelEachValue[10];

                byte[] buffer = new byte[60];
                fixed (byte* pbuf = buffer, rbuf = _sensorValue2)
                {
                    Win32.memcpy(pbuf, rbuf, buffer.Length * sizeof(byte));
                }

                for (int index = 0; index < 60; index += 6)
                {
                    byte[] temp = new byte[6];
                    for (int x = 0; x < temp.Length; x++)
                    {
                        temp[x] = buffer[index + x];
                    }

                    ChannelEachValue runrecord = new ChannelEachValue();
                    fixed (byte* pbuf = temp)
                    {
                        Win32.memcpy(&runrecord, pbuf, sizeof(ChannelEachValue));
                    }
                    records[index / 6] = runrecord;
                }
                return records;
            }
        }
        /// <summary>
        /// 3号通道传感器
        /// </summary>
        public ChannelEachValue[] Channel3
        {
            get
            {
                ChannelEachValue[] records = new ChannelEachValue[10];

                byte[] buffer = new byte[60];
                fixed (byte* pbuf = buffer, rbuf = _sensorValue3)
                {
                    Win32.memcpy(pbuf, rbuf, buffer.Length * sizeof(byte));
                }

                for (int index = 0; index < 60; index += 6)
                {
                    byte[] temp = new byte[6];
                    for (int x = 0; x < temp.Length; x++)
                    {
                        temp[x] = buffer[index + x];
                    }

                    ChannelEachValue runrecord = new ChannelEachValue();
                    fixed (byte* pbuf = temp)
                    {
                        Win32.memcpy(&runrecord, pbuf, sizeof(ChannelEachValue));
                    }
                    records[index / 6] = runrecord;
                }
                return records;
            }
        }
        /// <summary>
        /// 4号通道传感器
        /// </summary>
        public ChannelEachValue[] Channel4
        {
            get
            {
                ChannelEachValue[] records = new ChannelEachValue[10];

                byte[] buffer = new byte[60];
                fixed (byte* pbuf = buffer, rbuf = _sensorValue4)
                {
                    Win32.memcpy(pbuf, rbuf, buffer.Length * sizeof(byte));
                }

                for (int index = 0; index < 60; index += 6)
                {
                    byte[] temp = new byte[6];
                    for (int x = 0; x < temp.Length; x++)
                    {
                        temp[x] = buffer[index + x];
                    }

                    ChannelEachValue runrecord = new ChannelEachValue();
                    fixed (byte* pbuf = temp)
                    {
                        Win32.memcpy(&runrecord, pbuf, sizeof(ChannelEachValue));
                    }
                    records[index / 6] = runrecord;
                }
                return records;
            }
        }

        
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ChannelEachValue
    {
        #region 结构体数据字段
        //每60byte为一个从机的全部传感器报文【最多10路】
        //每6个字节为一个传感器数据：4byte为测量值[float]，2byte为单位

        float measureValue;//测量值
        fixed byte unit[2];//单位
        #endregion

        #region 属性
        /// <summary>
        /// 传感器测量值
        /// </summary>
        public float MeasureValue
        {
            get { return measureValue; }

            set { measureValue = value; }
        }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit
        {
            get
            {
                byte[] temp = new byte[2];
                fixed (byte* pFile = unit)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (pFile[i] == 0)
                        {
                            break;
                        }
                        temp[i]=pFile[i];
                    }
                }

                ushort unitCode = 0;
                unitCode = (ushort)(unitCode ^ temp[0]);//将temp[0]赋值给低8位
                unitCode = (ushort)(unitCode << 8); //低8位移动到高8位
                unitCode = (ushort)(unitCode ^ temp[1]);

                if (Enum.IsDefined(typeof(DataModule.EnumUnit), Convert.ToInt32(unitCode)))
                {
                    DataModule.EnumUnit enmu = (DataModule.EnumUnit)unitCode;
                    return DataModule.EnumShell.GetShell<DataModule.EnumUnit>(enmu).Description;
                }

                return "";
            }

            set
            {
                char[] charArray = value.ToCharArray();
                fixed (byte* pFile = unit)
                {
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        if (i >= 2)
                            break;
                        pFile[i] = (byte)charArray[i];
                    }
                }
            }
        }

        /// <summary>
        /// 示指显示
        /// </summary>
        public string ValueAndUnit
        {
            get { return measureValue + Unit; }
        }
        #endregion
    }


    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ALLSensorValue
    {
        #region 结构体数据字段
        //共240byte，4个通道，每个通道10个传感器，每个传感器6byte报文
        fixed byte _sensorValue[4 * 10 * 6];
        #endregion

        /// <summary>
        /// 传感器实时数据（共40个）
        /// </summary>
        public ObservableCollection<SensorRealValue> SensorList
        {
            get
            {
                ObservableCollection<SensorRealValue> records = new ObservableCollection<SensorRealValue>();

                byte[] buffer = new byte[240];
                fixed (byte* pbuf = buffer, rbuf = _sensorValue)
                {
                    Win32.memcpy(pbuf, rbuf, buffer.Length * sizeof(byte));
                }

                for (int index = 0; index < 240; index += 6)
                {
                    byte[] tempValue = new byte[4];
                    for (int x = 0; x < 4; x++)
                    {
                        tempValue[x] = buffer[index + x];
                    }

                    byte[] tempUnit = new byte[2];
                    for (int y = 0; y < 2; y++)
                    {
                        tempUnit[y] = buffer[index + 4 + y];
                    }

                    //查看当前计算机存储数据格式是否为小端格式，若不是，则反转为小端格式
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(tempValue);
                    }

                    byte[] temp = new byte[tempValue.Length + tempUnit.Length];
                    temp = tempValue.Concat(tempUnit).ToArray();

                    ChannelEachValue runRecord = new ChannelEachValue();
                    fixed (byte* pbuf = temp)
                    {
                        Win32.memcpy(&runRecord, pbuf, sizeof(ChannelEachValue));
                    }

                    SensorRealValue realValue = new SensorRealValue();
                    realValue.SensorID = index / 6 + 1;
                    realValue.SensorValue = runRecord.MeasureValue;
                    realValue.SensorUnit = runRecord.Unit;

                    records.Add(realValue);
                }
                return records;
            }
        }
    }

    /// <summary>
    /// 传感器实时数据
    /// </summary>
    public class SensorRealValue
    {
        private int _sensorID;
        private float _sensorValue;
        private string _sensorUnit;

        public int SensorID
        {
            get { return _sensorID; }
            set { _sensorID = value; }
        }

        public float SensorValue
        {
            get { return _sensorValue; }
            set { _sensorValue = value; }
        }

        public string SensorUnit
        {
            get { return _sensorUnit; }
            set { _sensorUnit = value; }
        }

        public string ValueAndUnit
        {
            get { return _sensorValue + _sensorUnit; }
        }
    }
}
