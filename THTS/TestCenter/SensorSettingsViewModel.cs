using THTS.MVVM;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using THTS.SerialPort;
using THTS.DataAccess;
using System.Windows;
using THTS.DataModule;

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

        private ObservableCollection<TestPositionModule> _positionList = new ObservableCollection<TestPositionModule>();
        /// <summary>
        /// 测点分布菜单列表
        /// </summary>
        public ObservableCollection<TestPositionModule> PositionList
        {
            get { return _positionList; }
            set { _positionList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Sensor> _sensorsList = new ObservableCollection<Sensor>();
        /// <summary>
        /// 传感器信息列表
        /// </summary>
        public ObservableCollection<Sensor> SensorsList
        {
            get { return _sensorsList; }
            set { _sensorsList = value; OnPropertyChanged(); }
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

        public SensorSettingsViewModel(TestInfo Info)
        {
            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();
            ToleranceInfo.RecordSN = Info.RecordSN;

            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
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
                    module.TestTemperatureID = "抽测" + i;
                    TestTemperatureList.Add(module);
                }
            }

            _testPositionList = new List<string>();
            _testPositionList.Add("9测点分布体");
            _testPositionList.Add("15测点分布体");
            SelectedTestPosition = _testPositionList[0];

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
                channel1.IsOnline = state.Channel1.IsOnline;
                channel1.ChannelName = "通道1：" + state.Channel1.ChannelName;
                channel1.SensorList = new List<Sensor>();

                Channel channel2 = new Channel();
                channel2.IsOnline = state.Channel2.IsOnline;
                channel2.ChannelName = "通道2：" + state.Channel2.ChannelName;
                channel2.SensorList = new List<Sensor>();

                Channel channel3 = new Channel();
                channel3.IsOnline = state.Channel3.IsOnline;
                channel3.ChannelName = "通道3：" + state.Channel3.ChannelName;
                channel3.SensorList = new List<Sensor>();

                Channel channel4 = new Channel();
                channel4.IsOnline = state.Channel4.IsOnline;
                channel4.ChannelName = "通道4：" + state.Channel4.ChannelName;
                channel4.SensorList = new List<Sensor>();

                for (int i = 1; i < 41; i++)
                {
                    if (i <= 10)
                    {
                        Sensor sensor1 = new Sensor();
                        sensor1.ChannelID = 1;
                        sensor1.SensorID = i;
                        sensor1.Type = state.Channel1.ChannelType;
                        channel1.SensorList.Add(sensor1);

                        if (state.Channel1.IsOnline)
                        {
                            _sensorsList.Add(sensor1);
                        }
                    }
                    
                    if(i > 10 && i <= 20)
                    {
                        Sensor sensor2 = new Sensor();
                        sensor2.ChannelID = 2;
                        sensor2.SensorID = i;
                        sensor2.Type = state.Channel2.ChannelType;
                        channel2.SensorList.Add(sensor2);

                        if (state.Channel2.IsOnline)
                        {
                            _sensorsList.Add(sensor2);
                        }
                    }

                    if (i > 20 && i <= 30)
                    {
                        Sensor sensor3 = new Sensor();
                        sensor3.ChannelID = 3;
                        sensor3.SensorID = i;
                        sensor3.Type = state.Channel3.ChannelType;
                        channel3.SensorList.Add(sensor3);

                        if (state.Channel3.IsOnline)
                        {
                            _sensorsList.Add(sensor3);
                        }
                    }

                    if (i > 30)
                    {
                        Sensor sensor4 = new Sensor();
                        sensor4.ChannelID = 4;
                        sensor4.SensorID = i;
                        sensor4.Type = state.Channel4.ChannelType;
                        channel4.SensorList.Add(sensor4);

                        if (state.Channel4.IsOnline)
                        {
                            _sensorsList.Add(sensor4);
                        }
                    }
                }
                _channelList.Clear();
                _channelList.Add(channel1);
                _channelList.Add(channel2);
                _channelList.Add(channel3);
                _channelList.Add(channel4);
            }

            instrument.Close();

            TestPositionChanged();
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

                PositionList.Clear();

                for (int i = 0; i < 8; i++)
                {
                    TestPositionModule item = new TestPositionModule();
                    item.SensorsList = SensorsList;
                    item.TestPositionName = Convert.ToChar(65 + i).ToString();
                    PositionList.Add(item);
                }

                TestPositionModule itemO = new TestPositionModule();
                itemO.SensorsList = SensorsList;
                itemO.TestPositionName = "O";
                PositionList.Add(itemO);

                TestPositionModule itemJia = new TestPositionModule();
                itemJia.SensorsList = SensorsList;
                itemJia.TestPositionName = "甲";
                PositionList.Add(itemJia);

                TestPositionModule itemYi = new TestPositionModule();
                itemYi.SensorsList = SensorsList;
                itemYi.TestPositionName = "乙";
                PositionList.Add(itemYi);

                TestPositionModule itemBing = new TestPositionModule();
                itemBing.SensorsList = SensorsList;
                itemBing.TestPositionName = "丙";
                PositionList.Add(itemBing);

            }
            else if (SelectedTestPosition.Contains("15"))
            {
                UC9Visibility = Visibility.Hidden;
                UC15Visibility = Visibility.Visible;

                PositionList.Clear();

                for (int i = 0; i < 15; i++)
                {
                    TestPositionModule item = new TestPositionModule();
                    item.SensorsList = SensorsList;
                    item.TestPositionName = Convert.ToChar(65 + i).ToString();
                    PositionList.Add(item);
                }

                TestPositionModule itemJia = new TestPositionModule();
                itemJia.SensorsList = SensorsList;
                itemJia.TestPositionName = "甲";
                PositionList.Add(itemJia);

                TestPositionModule itemYi = new TestPositionModule();
                itemYi.SensorsList = SensorsList;
                itemYi.TestPositionName = "乙";
                PositionList.Add(itemYi);

                TestPositionModule itemBing = new TestPositionModule();
                itemBing.SensorsList = SensorsList;
                itemBing.TestPositionName = "丙";
                PositionList.Add(itemBing);

                TestPositionModule itemDing = new TestPositionModule();
                itemDing.SensorsList = SensorsList;
                itemDing.TestPositionName = "丁";
                PositionList.Add(itemDing);
            }
        }

        /// <summary>
        /// 下一步方法
        /// </summary>
        private void Start()
        {
            ToleranceInfo.TemperatureList.Clear();
            for (int i = 0; i < TestTemperatureList.Count; i++)
            {
                if(TestTemperatureList[i].IsChecked == true)
                {
                    double temp = 0.0;
                    if(Double.TryParse(TestTemperatureList[i].TemperatureValue, out temp))
                    {
                        TestTemperatureList[i].TemperatureDoubleValue = temp;
                        ToleranceInfo.TemperatureList.Add(TestTemperatureList[i]);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("温度数据异常："+ TestTemperatureList[i].TestTemperatureID + TestTemperatureList[i].TemperatureValue);
                        return;
                    }
                }

            }

            ToleranceInfo.PositionType = SelectedTestPosition.Contains("9") ? 9 : 15;

            if (!CheckTestPostion())
            {
                return;
            }

            if (!DataAccess.EntityDAO.TemperatureToleranceDAO.SaveOrUpdate(ToleranceInfo))
            {
                System.Windows.MessageBox.Show("测试信息保存失败！");
                return;
            }

            SensorTest test = new SensorTest(ToleranceInfo);
            test.ShowDialog();
        }

        /// <summary>
        /// 检查测点传感器布置是否重复
        /// </summary>
        private bool CheckTestPostion()
        {
            ToleranceInfo.PositionList.Clear();
            Dictionary<int, string> positionDic = new Dictionary<int, string>();

            for (int i = 0; i < PositionList.Count; i++)
            {
                if(PositionList[i].TestPositionID == null)
                {
                    continue;
                }

                if (positionDic.ContainsKey(PositionList[i].TestPositionID.SensorID))
                {
                    System.Windows.MessageBox.Show("同一个传感器不允许放置到多个位置！");
                    return false;
                }

                positionDic.Add(PositionList[i].TestPositionID.SensorID, PositionList[i].TestPositionName);

                if (!ToleranceInfo.PositionList.ContainsKey(PositionList[i].TestPositionName))
                {
                    ToleranceInfo.PositionList.Add(PositionList[i].TestPositionName, PositionList[i].TestPositionID);
                }
                else
                {
                    ToleranceInfo.PositionList[PositionList[i].TestPositionName] = PositionList[i].TestPositionID;
                }
            }

            return true;

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
