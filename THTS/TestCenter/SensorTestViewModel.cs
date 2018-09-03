using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using THTS.MVVM;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts;
using THTS.SerialPort;
using THTS.DataAccess;
using THTS.DataAccess.Entity;
using THTS.DataModule;
using System.Windows.Threading;
using THTS.DataAccess.EntityDAO;

namespace THTS.TestCenter
{
    public class SensorTestViewModel : NotifyObject
    {
        #region Command
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand StopCommand { get; private set; }
        #endregion

        private int temperatureIndex = 0;

        private int _tabItemIndex = 0;
        /// <summary>
        /// 数据采集面板序号
        /// </summary>
        public int TabItemIndex
        {
            get { return _tabItemIndex; }
            set { _tabItemIndex = value; OnPropertyChanged(); }
        }

        private string _currentTemperature;
        /// <summary>
        /// 当前测试温度
        /// </summary>
        public string CurrentTemperature
        {
            get { return _currentTemperature; }
            set { _currentTemperature = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SensorRealValue> _sensorList = new ObservableCollection<SensorRealValue>();
        /// <summary>
        /// 所有传感器实时信息
        /// </summary>
        public ObservableCollection<SensorRealValue> SensorList
        {
            get { return _sensorList; }
            set { _sensorList = value; OnPropertyChanged(); }
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

        private Dictionary<string, Sensor> _positionName;
        /// <summary>
        /// 测点分布与传感器对应关系
        /// </summary>
        public Dictionary<string, Sensor> PositionName
        {
            get { return _positionName; }
            set { _positionName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TestDataList> _testResultDataList = new ObservableCollection<TestDataList>();
        /// <summary>
        /// 数据采集结果
        /// </summary>
        public ObservableCollection<TestDataList> TestResultDataList
        {
            get { return _testResultDataList; }
            set { _testResultDataList = value; OnPropertyChanged(); }
        }

        private TemperatureTolerance tol;
        /// <summary>
        /// 测试参数
        /// </summary>
        public TemperatureTolerance Tol
        {
            get { return tol; }
            set { tol = value; OnPropertyChanged(); }
        }

        private DateTime currTime;
        bool timeout = false;


        private bool _startEnable = true;
        /// <summary>
        /// 开始按钮是否有效
        /// </summary>
        public bool StartEnable
        {
            get { return _startEnable; }
            set { _startEnable = value; OnPropertyChanged(); }
        }

        private int _barValue = 0;
        /// <summary>
        /// 进度条当前进度
        /// </summary>
        public int BarValue
        {
            get { return _barValue; }
            set { _barValue = value; OnPropertyChanged(); }
        }

        private string _barTime;
        /// <summary>
        /// 进度条倒计时
        /// </summary>
        public string BarTime
        {
            get { return _barTime; }
            set { _barTime = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 通讯实例
        /// </summary>
        iInstrument instrument = new iInstrument("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


        /// <summary>
        /// 构造函数
        /// </summary>
        public SensorTestViewModel(TemperatureTolerance tolerance)
        {
            tol = tolerance;

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);

            TestPositionChanged(tolerance.PositionType);

            PositionName = tolerance.PositionList;

            TestDataTemperatureTitle(tolerance.TemperatureList);

            //获取串口配置信息
            DataAccess.Setting settings = DataAccess.SettingsDAO.GetData();

            instrument = new iInstrument(settings.PortName, settings.BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            SyncData();
        }

        /// <summary>
        /// 采集数据的温度标题更新
        /// </summary>
        public void TestDataTemperatureTitle(ObservableCollection<TestTemperatureModule> templist)
        {
            for (int i = 0; i < templist.Count; i++)
            {
                TestDataList data = new TestDataList
                {
                    TemperatureName = templist[i].TestTemperatureID,
                    TemperatureValue = templist[i].TemperatureValue,
                    HumidityValue = templist[i].HumidityValue
                };
                TestResultDataList.Add(data);
            }

            if (templist.Count > 0)
            {
                CurrentTemperature = templist[0].TemperatureValue;
                TabItemIndex = 0;
            }
        }

        /// <summary>
        /// 实时刷新数据
        /// </summary>
        public void SyncData()
        {
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    ALLSensorValue allSensorValue = new ALLSensorValue();
                    if (instrument.GetALLSensorValue(out allSensorValue))
                    {
                        ObservableCollection<SensorRealValue> _tempList = allSensorValue.SensorList;

                        for (int i = 0; i < _tempList.Count; i++)
                        {
                            #region Test
                            SensorRealValue real = new SensorRealValue();
                            real.SensorID = i + 1;
                            real.SensorValue = (float)(50 + (new Random(Guid.NewGuid().GetHashCode()).Next(-100,100)) * 0.01);
                            real.SensorUnit = "℃";
                            #endregion

                            //SensorRealValue real = _tempList[i];

                            this.DispatcherInvoke(() =>
                            {
                                if (_sensorList.Count > i)
                                {
                                    _sensorList[i] = real;
                                }
                                else
                                {
                                    _sensorList.Add(real);
                                }
                            });
                        }
                    }
                }
            }));
            thrP.IsBackground = true;
            thrP.Start();
        }

        /// <summary>
        /// 开始采集数据
        /// </summary>
        private void Start()
        {
            DispatcherTimer timerBar = new DispatcherTimer();
            timerBar.Interval = TimeSpan.FromSeconds(1);
            timerBar.Tick += ProcessBarChange;
            currTime = DateTime.Now.AddMinutes(tol.TestTimeSpan);
            timerBar.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(tol.TestSampleInterval);
            timer.Tick += CollectData;
            timer.Start();

            DateTime now = DateTime.Now;
            timeout = false;
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                StartEnable = false;

                while (!timeout)
                {
                    BarValue = (int)(DateTime.Now - now).TotalSeconds * 100 / (tol.TestTimeSpan * 60);
                    timeout = BarValue >= 100;
                    Thread.Sleep(500);
                }

                timerBar.Stop();
                timer.Stop();
                TestDataDAO.Save(TestResultDataList[temperatureIndex].DataList);
                StartEnable = true;

                if (TestResultDataList.Count > temperatureIndex + 1)
                {
                    temperatureIndex = temperatureIndex + 1;
                    TabItemIndex = TabItemIndex + 1;
                    CurrentTemperature = TestResultDataList[temperatureIndex].TemperatureValue;
                }
            }));
            thrP.IsBackground = true;
            thrP.Start();
        }

        /// <summary>
        /// 进度条变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBarChange(object sender, EventArgs e)
        {
            TimeSpan temp = currTime - DateTime.Now;
            BarTime = temp.Hours.ToString("00") + ":" + temp.Minutes.ToString("00") + ":" + temp.Seconds.ToString("00");
        }

        /// <summary>
        /// 单点数据采集事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectData(object sender, EventArgs e)
        {
            TestData data = new TestData
            {
                RecordSN = tol.RecordSN,
                TemperatureName = TestResultDataList[temperatureIndex].TemperatureName,
                TemperatureValue = CurrentTemperature,
                HumidityValue = TestResultDataList[temperatureIndex].HumidityValue,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Count = TestResultDataList[temperatureIndex].DataList.Count + 1,
                DeviceTemperature = CurrentTemperature,
                DeviceHumidity = TestResultDataList[temperatureIndex].HumidityValue,

                A = SetValueForPosition("A"),
                B = SetValueForPosition("B"),
                C = SetValueForPosition("C"),
                D = SetValueForPosition("D"),
                E = SetValueForPosition("E"),
                F = SetValueForPosition("F"),
                G = SetValueForPosition("G"),
                H = SetValueForPosition("H"),
                I = SetValueForPosition("I"),
                J = SetValueForPosition("J"),
                K = SetValueForPosition("K"),
                L = SetValueForPosition("L"),
                M = SetValueForPosition("M"),
                N = SetValueForPosition("N"),
                O = SetValueForPosition("O"),
                Jia = SetValueForPosition("甲"),
                Yi = SetValueForPosition("乙"),
                Bing = SetValueForPosition("丙"),
                Ding = SetValueForPosition("丁")
            };

            TestResultDataList[temperatureIndex].DataList.Add(data);
        }

        private float SetValueForPosition(string position)
        {
            if (PositionName.ContainsKey(position))
            {
                return SensorList[PositionName[position].SensorID - 1].SensorValue;
            }

            return -1000;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        private void Stop()
        {
            timeout = true;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        private void GetLiveData()
        {

        }

        /// <summary>
        /// 测点分布切换
        /// </summary>
        private void TestPositionChanged(int testPositionType)
        {
            if (testPositionType == 9)
            {
                UC9Visibility = Visibility.Visible;
                UC15Visibility = Visibility.Collapsed;
            }
            else if (testPositionType == 15)
            {
                UC9Visibility = Visibility.Collapsed;
                UC15Visibility = Visibility.Visible;
            }
        }
    }
}
