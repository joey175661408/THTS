using THTS.MVVM;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using THTS.SerialPort;
using THTS.DataAccess;

namespace THTS.TestCenter
{
    public class SensorSettingsViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand SensorGetCommand { get; private set; }
        public IDelegateCommand SensorSelectCommand { get; private set; }
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性

        private ObservableCollection<Channel> _channelList = new ObservableCollection<Channel>();
        /// <summary>
        /// 所有通道信息
        /// </summary>
        public ObservableCollection<Channel> ChannelList
        {
            get { return _channelList; }
            set { _channelList = value; OnPropertyChanged(); }
        }

        private TemperatureTolerance _toleranceInfo;
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public TemperatureTolerance ToleranceInfo
        {
            get { return _toleranceInfo; }
            set { _toleranceInfo = value; OnPropertyChanged(); }
        }

        private Sensor _selectSensor = new Sensor();
        /// <summary>
        /// 当前选中的传感器
        /// </summary>
        public Sensor SelectSensor
        {
            get { return _selectSensor; }
            set { _selectSensor = value; OnPropertyChanged(); }
        }

        iInstrument instrument = new iInstrument("COM4", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

        #endregion

        public SensorSettingsViewModel()
        {
            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();

            SensorGetCommand = new DelegateCommand(SensorGet);
            SensorSelectCommand = new DelegateCommand(SensorSelect);
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);
        }

        #region 方法

        /// <summary>
        /// 获取通道信息
        /// </summary>
        private void SensorGet()
        {
            if (!instrument.Open())
            {
                return;
            }

            ChannelState state = new ChannelState();
            if(instrument.GetSensorState(out state))
            {
                Channel channel1 = new Channel();
                channel1.IsOnlie = state.Channel1.IsOnline;
                channel1.ChannelName = "通道1：" + state.Channel1.ChannelName;
                channel1.SensorList = new List<Sensor>();

                Channel channel2 = new Channel();
                channel2.IsOnlie = state.Channel2.IsOnline;
                channel2.ChannelName = "通道2：" + state.Channel2.ChannelName;
                channel2.SensorList = new List<Sensor>();

                Channel channel3 = new Channel();
                channel3.IsOnlie = state.Channel3.IsOnline;
                channel3.ChannelName = "通道3：" + state.Channel3.ChannelName;
                channel3.SensorList = new List<Sensor>();

                Channel channel4 = new Channel();
                channel4.IsOnlie = state.Channel4.IsOnline;
                channel4.ChannelName = "通道4：" + state.Channel4.ChannelName;
                channel4.SensorList = new List<Sensor>();

                for (int i = 1; i < 41; i++)
                {
                    if (i <= 10)
                    {
                        Sensor sensor1 = new Sensor();
                        sensor1.ChannelID = 1;
                        sensor1.IsOnline = state.Channel1.IsOnline;
                        sensor1.SensorID = i;
                        sensor1.Type = state.Channel1.ChannelType;
                        channel1.SensorList.Add(sensor1);
                    }
                    
                    if(i > 10 && i <= 20)
                    {
                        Sensor sensor2 = new Sensor();
                        sensor2.ChannelID = 2;
                        sensor2.IsOnline = state.Channel2.IsOnline;
                        sensor2.SensorID = i;
                        sensor2.Type = state.Channel2.ChannelType;
                        channel2.SensorList.Add(sensor2);
                    }

                    if (i > 20 && i <= 30)
                    {
                        Sensor sensor3 = new Sensor();
                        sensor3.ChannelID = 3;
                        sensor3.IsOnline = state.Channel3.IsOnline;
                        sensor3.SensorID = i;
                        sensor3.Type = state.Channel3.ChannelType;
                        channel3.SensorList.Add(sensor3);
                    }

                    if (i > 30)
                    {
                        Sensor sensor4 = new Sensor();
                        sensor4.ChannelID = 4;
                        sensor4.IsOnline = state.Channel4.IsOnline;
                        sensor4.SensorID = i;
                        sensor4.Type = state.Channel4.ChannelType;
                        channel4.SensorList.Add(sensor4);
                    }
                }
                _channelList.Clear();
                _channelList.Add(channel1);
                _channelList.Add(channel2);
                _channelList.Add(channel3);
                _channelList.Add(channel4);
            }

            instrument.Close();
        }

        /// <summary>
        /// 选择传感器
        /// </summary>
        private void SensorSelect()
        {
            DeviceCenter.DeviceCenter deviceSelect = new DeviceCenter.DeviceCenter(true);
            bool? result = deviceSelect.ShowDialog();
            if (result.HasValue && result.Value && deviceSelect.DeviceSelectList != null)
            {
                Thread thr = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        this.DispatcherInvoke(() =>
                        {
                            if (deviceSelect.DeviceSelectList != null && deviceSelect.DeviceSelectList.Count > 0)
                            {
                                SelectSensor.DeviceID = deviceSelect.DeviceSelectList[0].Id;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                    }
                }));
                thr.IsBackground = true;
                thr.Start();
            }
        }

        /// <summary>
        /// 开始测试方法
        /// </summary>
        private void Start()
        {
            if (!DataAccess.EntityDAO.TemperatureToleranceDAO.SaveOrUpdate(ToleranceInfo))
            {
                System.Windows.MessageBox.Show("测试信息保存失败！");
                return;
            }

            SensorTest test = new SensorTest();
            test.ShowDialog();
        }

        /// <summary>
        /// 关闭窗体方法
        /// </summary>
        private void Close()
        {
        }
        #endregion
    }
}
