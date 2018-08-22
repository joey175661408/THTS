using THTS.MVVM;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using THTS.SerialPort;
using THTS.DataAccess;
using System.Windows;

namespace THTS.TestCenter
{
    public class SensorSettingsViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand SensorGetCommand { get; private set; }
        /// <summary>
        /// 测试点分布切换命令
        /// </summary>
        public IDelegateCommand SelectedPositionCommand { get; private set; }
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

        private ObservableCollection<DataModule.TestTemperatureModule> _testTemperatureList = new ObservableCollection<DataModule.TestTemperatureModule>();
        /// <summary>
        /// 温度模块信息
        /// </summary>
        public ObservableCollection<DataModule.TestTemperatureModule> TestTemperatureList
        {
            get { return _testTemperatureList; }
            set { _testTemperatureList = value; OnPropertyChanged(); }
        }

        private Visibility _uC9Visibility = Visibility.Visible;
        /// <summary>
        /// 9测点分布示意图 
        /// </summary>
        public Visibility UC9Visibility
        {
            get { return _uC9Visibility; }
            set { _uC9Visibility = value; OnPropertyChanged(); }
        }

        private Visibility _uC15Visibility = Visibility.Visible;
        /// <summary>
        /// 15测点分布示意图 
        /// </summary>
        public Visibility UC15Visibility
        {
            get { return _uC15Visibility; }
            set { _uC15Visibility = value; OnPropertyChanged(); }
        }

        private List<string> _testPositionList;
        /// <summary>
        /// 温场测点分布体列表
        /// </summary>
        public List<string> TestPositionList
        {
            get { return _testPositionList; }
            set { _testPositionList = value; OnPropertyChanged(); }
        }

        private string _selectedTestPosition = string.Empty;
        /// <summary>
        /// 选中的测点分布体
        /// </summary>
        public string SelectedTestPosition
        {
            get { return _selectedTestPosition; }
            set { _selectedTestPosition = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _sensorIDList = new ObservableCollection<string>();
        /// <summary>
        /// 传感器ID列表
        /// </summary>
        public ObservableCollection<string> SensorIDList
        {
            get { return _sensorIDList; }
            set { _sensorIDList = value; OnPropertyChanged(); }
        }

        private TemperatureTolerance _toleranceInfo = new TemperatureTolerance();
        /// <summary>
        /// 当前测试信息
        /// </summary>
        public TemperatureTolerance ToleranceInfo
        {
            get { return _toleranceInfo; }
            set { _toleranceInfo = value; OnPropertyChanged(); }
        }

        iInstrument instrument = new iInstrument("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

        #endregion

        public SensorSettingsViewModel()
        {
            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();

            for (int i = 0; i < 10; i++)
            {
                if(i == 0)
                {
                    DataModule.TestTemperatureModule module = new DataModule.TestTemperatureModule();
                    module.IsChecked = true;
                    module.TestTemperatureID = "下限";
                    TestTemperatureList.Add(module);
                }
                else if (i == 9)
                {
                    DataModule.TestTemperatureModule module = new DataModule.TestTemperatureModule();
                    module.IsChecked = true;
                    module.TestTemperatureID = "上限";
                    TestTemperatureList.Add(module);
                }
                else
                {
                    DataModule.TestTemperatureModule module = new DataModule.TestTemperatureModule();
                    module.IsChecked = false;
                    module.TestTemperatureID = "抽测" + i ;
                    TestTemperatureList.Add(module);
                }
            }

            _testPositionList = new List<string>();
            _testPositionList.Add("9测点分布体");
            _testPositionList.Add("15测点分布体");
            SelectedTestPosition = _testPositionList[0];

            for (int i = 1; i <= 40; i++)
            {
                _sensorIDList.Add(i.ToString());
            }

            SelectedPositionCommand = new DelegateCommand(TestPositionChanged);
            SensorGetCommand = new DelegateCommand(SensorGet);
            StartCommand = new DelegateCommand(Start);
            CloseCommand = new DelegateCommand(Close);

            //获取串口配置信息
            DataAccess.Setting settings = DataAccess.SettingsDAO.GetData();

            instrument = new iInstrument(settings.PortName, settings.BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
        }

        #region 方法

        /// <summary>
        /// 获取通道信息
        /// </summary>
        private void SensorGet()
        {
            //if (!instrument.Open())
            //{
            //    return;
            //}

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
        /// 测点分布切换
        /// </summary>
        private void TestPositionChanged()
        {
            if (SelectedTestPosition.Contains("9"))
            {
                UC9Visibility = Visibility.Visible;
                UC15Visibility = Visibility.Hidden;
            }
            else if (SelectedTestPosition.Contains("15"))
            {
                UC9Visibility = Visibility.Hidden;
                UC15Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 下一步方法
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < TestTemperatureList.Count; i++)
            {
                if(TestTemperatureList[i].IsChecked == true)
                {
                    double temp = 0.0;
                    if(Double.TryParse(TestTemperatureList[i].TemperatureValue, out temp))
                    {
                        ToleranceInfo.TemperatureList.Add(temp);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("温度数据异常："+ TestTemperatureList[i].TestTemperatureID + TestTemperatureList[i].TemperatureValue);
                        return;
                    }
                }

            }


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
