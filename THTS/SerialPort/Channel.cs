using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess;
using THTS.MVVM;

namespace THTS.SerialPort
{
    public class Channel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Channel()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        private bool _isOnline;
        /// <summary>
        /// 通道是否在线
        /// </summary>
        public bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; }
        }

        private string _channelName;
        /// <summary>
        /// 通道名称
        /// </summary>
        public string ChannelName
        {
            get { return _channelName; }
            set { _channelName = value; }
        }

        private ObservableCollection<Sensor> _sensorList = new ObservableCollection<Sensor>();


        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Sensor> SensorList
        {
            get { return _sensorList; }
            set { _sensorList = value; }
        }
    }
}
