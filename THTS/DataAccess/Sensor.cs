using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.MVVM;

namespace THTS.DataAccess
{
    /// <summary>
    /// 传感器数据结构
    /// </summary>
    public class Sensor : INotifyPropertyChanged
    {
        private int channelID;
        private string type;
        private int sensorID;

        private string deviceID;
        private List<string> deviceIDList;

        private Device device;
        private ObservableCollection<Device> deviceList;

        public Sensor()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
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
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        /// <summary>
        /// 传感器ID列表
        /// </summary>
        public List<string> DeviceIDList
        {
            get { return deviceIDList; }
            set { deviceIDList = value; }
        }

        /// <summary>
        /// 选中的传感器
        /// </summary>
        public Device Device
        {
            get { return device; }
            set { device = value;}
        }

        /// <summary>
        /// 测点位置中下拉列表内容（通道中传感器序号+传感器出厂编号）
        /// </summary>
        public string SensorIDwithFactoryID
        {
            get 
            {
                if(Device == null)
                {
                    return SensorID + "：";
                }
                else
                {
                    return SensorID + "：" + device.DeviceIDWithFactoryNO; 
                }
            }
        }

        /// <summary>
        /// 设备列表(出厂编号)
        /// </summary>
        public ObservableCollection<Device> DeviceList
        {
            get { return deviceList; }
            set { deviceList = value; }
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
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
