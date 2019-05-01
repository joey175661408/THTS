using THTS.MVVM;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using THTS.SerialPort;
using THTS.DataAccess;
using System.Windows;
using THTS.DataModule;
using System.Windows.Forms;
using THTS.DataAccess.EntityDAO;

namespace THTS.TestCenter
{
    public class SensorSettingsMultiViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand SensorGetCommand { get; private set; }
        public IDelegateCommand DefaultGetCommand { get; private set; }
        public IDelegateCommand DefaultSaveCommand { get; private set; }
        /// <summary>
        /// 测试点分布切换命令
        /// </summary>
        public IDelegateCommand StartCommand { get; private set; }
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

        /// <summary>
        /// 传感器ID列表
        /// </summary>
        private List<string> deviceIDList = new List<string>();

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

        public SensorSettingsMultiViewModel(TestInfo Info)
        {
            ToleranceInfo = DataAccess.EntityDAO.TemperatureToleranceDAO.GetToleranceInfoData();
            ToleranceInfo.RecordSN = Info.RecordSN;
            ToleranceInfo.Info = Info;

            for (int i = 0; i < 4; i++)
            {
                TestTemperatureModule module = new TestTemperatureModule();
                module.IsChecked = false;
                module.TestTemperatureID = "设置测量" + (i + 1);

                if (i == 0)
                {
                    module.IsChecked = true;
                    module.TemperatureValue = Info.TemperatureLower;
                }
                else if (i == 3)
                {
                    module.IsChecked = true;
                    module.TemperatureValue = Info.TemperatureUpper;
                }

                TestTemperatureList.Add(module);
            }

            _testPositionList = new List<string>();
            _testPositionList.Add("9测点分布体");
            SelectedTestPosition = _testPositionList[0];

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

            SensorGetCommand = new DelegateCommand(SensorGet);
            DefaultGetCommand = new DelegateCommand(DefaultGet);
            DefaultSaveCommand = new DelegateCommand(DefaultSave);
            StartCommand = new DelegateCommand(Start);

            //获取串口配置信息
            DataAccess.Setting settings = DataAccess.SettingsDAO.GetData();

            instrument = new iInstrument(settings.PortName, settings.BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            //获取传感器ID列表
            deviceIDList = DeviceDAO.GetAllData().Select(t => t.FactoryNo).ToList();
        }

        #region 方法

        /// <summary>
        /// 获取通道信息
        /// </summary>
        private void SensorGet()
        {
            instrument.Close();

            if (!instrument.Open())
            {
                System.Windows.Forms.MessageBox.Show("串口打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

                for (int i = 1; i < 40; i++)
                {
                    if (i <= 9)
                    {
                        Sensor sensor1 = new Sensor();
                        sensor1.ChannelID = 1;
                        sensor1.SensorID = i;
                        sensor1.Type = state.Channel1.ChannelType;
                        sensor1.DeviceIDList = deviceIDList;
                        channel1.SensorList.Add(sensor1);
                        _sensorsList.Add(sensor1);
                    }
                    
                    if(i > 10 && i <= 19)
                    {
                        Sensor sensor2 = new Sensor();
                        sensor2.ChannelID = 2;
                        sensor2.SensorID = i;
                        sensor2.Type = state.Channel2.ChannelType;
                        sensor2.DeviceIDList = deviceIDList;
                        channel2.SensorList.Add(sensor2);
                        _sensorsList.Add(sensor2);
                    }

                    if (i > 20 && i <= 29)
                    {
                        Sensor sensor3 = new Sensor();
                        sensor3.ChannelID = 3;
                        sensor3.SensorID = i;
                        sensor3.Type = state.Channel3.ChannelType;
                        sensor3.DeviceIDList = deviceIDList;
                        channel3.SensorList.Add(sensor3);
                        _sensorsList.Add(sensor3);
                    }

                    if (i > 30)
                    {
                        Sensor sensor4 = new Sensor();
                        sensor4.ChannelID = 4;
                        sensor4.SensorID = i;
                        sensor4.Type = state.Channel4.ChannelType;
                        sensor4.DeviceIDList = deviceIDList;
                        channel4.SensorList.Add(sensor4);
                        _sensorsList.Add(sensor4);
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
        /// 获取默认参数
        /// </summary>
        private void DefaultGet()
        {
            if (ChannelList.Where(t=>t.IsOnline == true).Count() == 0)
            {
                System.Windows.MessageBox.Show("设备离线！");
                return;
            }

            //加载传感器默认配置
            ObservableCollection<Channel> ChannelListTemp = new ObservableCollection<Channel>();
            for (int i = 0; i < ChannelList.Count; i++)
            {
                ChannelListTemp.Add(ChannelList[i]);
            }

            for (int j = 1; j < 40; j++)
            {
                string tempID = MultiFileHelper.IniReadValue("SensorMulti", j.ToString());
                if (!string.IsNullOrEmpty(tempID))
                {
                    if (j < 10)
                    {
                        ChannelListTemp[0].SensorList[j - 1].DeviceID = tempID;
                    }
                    else if (j > 10 && j < 20)
                    {
                        ChannelListTemp[1].SensorList[j - 11].DeviceID = tempID;
                    }
                    else if (j > 20 && j < 30)
                    {
                        ChannelListTemp[2].SensorList[j - 21].DeviceID = tempID;
                    }
                    else
                    {
                        ChannelListTemp[3].SensorList[j - 31].DeviceID = tempID;
                    }

                }
            }

            ChannelList = ChannelListTemp;

            for (int i = 0; i < PositionList.Count; i++)
            {
                string tempID = MultiFileHelper.IniReadValue("Position", PositionList[i].TestPositionName);
                if (!string.IsNullOrEmpty(tempID))
                {
                    PositionList[i].TestPositionID = SensorsList[int.Parse(tempID) - 1];
                }
            }

            //加载温度默认配置
            ObservableCollection<DataModule.TestTemperatureModule> TestTemperatureListTemp = new ObservableCollection<TestTemperatureModule>();

            for (int i = 0; i < TestTemperatureList.Count; i++)
            {
                string tempID = MultiFileHelper.IniReadValue("Temperture", TestTemperatureList[i].TestTemperatureID);

                if (!string.IsNullOrEmpty(tempID))
                {
                    TestTemperatureList[i].IsChecked = true;
                    TestTemperatureList[i].TemperatureValue = tempID;
                }
                else
                {
                    TestTemperatureList[i].IsChecked = false;
                    TestTemperatureList[i].TemperatureValue = string.Empty;
                    TestTemperatureList[i].HumidityValue = string.Empty;
                }
                TestTemperatureListTemp.Add(TestTemperatureList[i]);
            }

            TestTemperatureList = TestTemperatureListTemp;
        }

        /// <summary>
        /// 保存默认参数
        /// </summary>
        private void DefaultSave()
        {
            try
            {
                //记录默认传感器ID
                for (int i = 0; i < SensorsList.Count; i++)
                {
                    MultiFileHelper.IniWriteValue("SensorMulti", SensorsList[i].SensorID.ToString(), SensorsList[i].DeviceID);
                }

                //记录默认测点分布
                for (int i = 0; i < PositionList.Count; i++)
                {
                    if (PositionList[i].TestPositionID == null)
                    {
                        continue;
                    }

                    MultiFileHelper.IniWriteValue("Position", PositionList[i].TestPositionName, PositionList[i].TestPositionID.SensorID.ToString());
                }

                //记录默认测点温度
                for (int i = 0; i < TestTemperatureList.Count; i++)
                {
                    if (TestTemperatureList[i].IsChecked.HasValue && TestTemperatureList[i].IsChecked == true)
                    {
                        MultiFileHelper.IniWriteValue("Temperture", TestTemperatureList[i].TestTemperatureID,TestTemperatureList[i].TemperatureValue );
                    }
                }

                System.Windows.MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 下一步方法
        /// </summary>
        private void Start()
        {
            if (ChannelList.Where(t => t.IsOnline == true).Count() == 0)
            {
                System.Windows.MessageBox.Show("设备离线！");
                return;
            }

            ToleranceInfo.TemperatureList.Clear();
            for (int i = 0; i < TestTemperatureList.Count; i++)
            {
                if(TestTemperatureList[i].IsChecked == true)
                {
                    double temp = 0.0;
                    if(Double.TryParse(TestTemperatureList[i].TemperatureValue, out temp))
                    {
                        ToleranceInfo.TemperatureList.Add(TestTemperatureList[i]);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("温度数据异常："+ TestTemperatureList[i].TestTemperatureID + TestTemperatureList[i].TemperatureValue);
                        return;
                    }
                }

            }

            ToleranceInfo.PositionType =  9;

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

            if(ToleranceInfo.Info.TemperatuerVisibility == Visibility.Visible && !ToleranceInfo.PositionList.ContainsKey("O"))
            {
                System.Windows.MessageBox.Show("温度中心点位置【O】未放置传感器！");
                return false;
            }

            return true;

        }

        #endregion
    }
}
