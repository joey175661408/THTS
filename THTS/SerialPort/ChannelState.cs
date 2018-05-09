using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace THTS.SerialPort
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ChannelState
    {
        #region 结构体数据字段
        //共80byte，每20byte为一个从机的状态信息

        ChannelEachState channel1;
        ChannelEachState channel2;
        ChannelEachState channel3;
        ChannelEachState channel4;
        #endregion

        #region 属性
        /// <summary>
        /// 1号通道传感器
        /// </summary>
        public ChannelEachState Channel1
        {
            get { return channel1; }
            set { channel1 = value; }
        }
        /// <summary>
        /// 2号通道传感器
        /// </summary>
        public ChannelEachState Channel2
        {
            get { return channel2; }
            set { channel2 = value; }
        }
        /// <summary>
        /// 3号通道传感器
        /// </summary>
        public ChannelEachState Channel3
        {
            get { return channel3; }
            set { channel3 = value; }
        }
        /// <summary>
        /// 4号通道传感器
        /// </summary>
        public ChannelEachState Channel4
        {
            get { return channel4; }
            set { channel4 = value; }
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct ChannelEachState
    {
        #region 结构体数据字段
        //每20byte为一个从机的状态信息，其中：
        //Byte1：为在线与否
        //Byte2~BYTE17:从机设备型号
        //Byte18~BYTE20:3个字节备用

        byte isOnline;//是否在线
        fixed byte channelType[16];//从机设备类型
        fixed byte standby[3];//备用
        #endregion

        /// <summary>
        /// 通道是否在线
        /// </summary>
        public bool IsOnline
        {
            get { return isOnline == 1; }
            set { isOnline = (byte)(value ? 1 : 0); }
        }

        /// <summary>
        /// 通道设备类型
        /// </summary>
        public string ChannelType
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                fixed (byte* pFile = channelType)
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
            set
            {
                char[] charArray = value.ToCharArray();
                fixed (byte* pFile = channelType)
                {
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        if (i >= 16)
                            break;
                        pFile[i] = (byte)charArray[i];
                    }
                }
            }

        }

        /// <summary>
        /// 通道显示名称
        /// </summary>
        public string ChannelName
        {
            get
            {
                if (IsOnline)
                {
                    return ChannelType;
                }
                else
                {
                    return "离线";
                }
            }
        }
    }
}
