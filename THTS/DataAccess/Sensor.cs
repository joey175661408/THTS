using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THTS.DataAccess
{
    /// <summary>
    /// 传感器数据结构
    /// </summary>
    public class Sensor
    {
        private bool isOnline;
        private string type;
        private int channelID;
        private int sensorID;
        private int deviceID;
        private string sensorPosition;
        private List<SensorData> sensorDataList;

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        /// <summary>
        /// 传感器类型
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 传感器所在通道ID
        /// </summary>
        public int ChannelID
        {
            get { return channelID; }
            set { channelID = value; }
        }

        /// <summary>
        /// 通道中的传感器ID序号
        /// </summary>
        public int SensorID
        {
            get { return sensorID; }
            set { sensorID = value; }
        }

        /// <summary>
        /// 软件中记录的传感器设备ID
        /// </summary>
        public int DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        /// <summary>
        /// 传感器所在温箱中的位置
        /// </summary>
        public string SensorPosition
        {
            get { return sensorPosition; }
            set { sensorPosition = value; }
        }

        /// <summary>
        /// 传感器数据集合
        /// </summary>
        public List<SensorData> SensorDataList
        {
            get { return sensorDataList; }
            set { sensorDataList = value; }
        }

    }

    /// <summary>
    /// 传感器温度点数据集合
    /// </summary>
    public class SensorData
    {
        private double tempertureControl;
        private List<SensorValue> sensorValueList;

        /// <summary>
        /// 当前控温点
        /// </summary>
        public double TemperatureControl
        {
            get { return tempertureControl; }
            set { tempertureControl = value; }
        }

        /// <summary>
        /// 传感器数据集合
        /// </summary>
        public List<SensorValue> SensorValueList
        {
            get { return sensorValueList; }
            set { sensorValueList = value; }
        }

    }

    /// <summary>
    /// 传感器瞬时数据
    /// </summary>
    public class SensorValue
    {
        private DateTime currentTime;
        private double currentValue;

        /// <summary>
        /// 当前时间点
        /// </summary>
        public DateTime CurrentTime
        {
            get { return currentTime; }
            set { currentTime = value; }
        }

        /// <summary>
        /// 当前示指
        /// </summary>
        public double CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }
    }
}
