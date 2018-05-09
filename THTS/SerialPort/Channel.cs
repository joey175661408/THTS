using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess;

namespace THTS.SerialPort
{
    public class Channel
    {
        private bool _isOnline;
        /// <summary>
        /// 通道是否在线
        /// </summary>
        public bool IsOnlie
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

        private List<Sensor> _sensorList = new List<Sensor>();
        /// <summary>
        /// 
        /// </summary>
        public List<Sensor> SensorList
        {
            get { return _sensorList; }
            set { _sensorList = value; }
        }
    }
}
