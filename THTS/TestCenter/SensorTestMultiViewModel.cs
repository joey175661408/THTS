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
    public class SensorTestMultiViewModel : NotifyObject
    {
        #region 开启调试模式
        /// <summary>
        /// 是否为调试模式
        /// </summary>
        private bool test = true;
        #endregion

        #region Command
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand StopCommand { get; private set; }
        public IDelegateCommand SaveAndCloseCommand { get; private set; }
        public IDelegateCommand ReturnCommand { get; private set; }
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

        private string _startOrPause = "▶";
        /// <summary>
        /// 开始按钮是否有效名称
        /// </summary>
        public string StartOrPause
        {
            get { return _startOrPause; }
            set { _startOrPause = value; OnPropertyChanged(); }
        }
        //暂停标志
        private bool pause = false;

        private bool _stopEnable = false;
        /// <summary>
        /// 中止按钮是否有效
        /// </summary>
        public bool StopEnable
        {
            get { return _stopEnable; }
            set { _stopEnable = value; OnPropertyChanged(); }
        }

        private bool _saveEnable = false;
        /// <summary>
        /// 保存按钮是否有效
        /// </summary>
        public bool SaveEnable
        {
            get { return _saveEnable; }
            set { _saveEnable = value; OnPropertyChanged(); }
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

        #region 温度曲线参数

        ObservableDataSource<Point> _lineRight = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源Right
        /// </summary>
        public ObservableDataSource<Point> LineRight
        {
            get { return _lineRight; }
            set { _lineRight = value; OnPropertyChanged(); }
        }

        ObservableDataSource<Point> _lineTop = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源Top
        /// </summary>
        public ObservableDataSource<Point> LineTop
        {
            get { return _lineTop; }
            set { _lineTop = value; OnPropertyChanged(); }
        }

        ObservableDataSource<Point> _lineBottom = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源Bottom
        /// </summary>
        public ObservableDataSource<Point> LineBottom
        {
            get { return _lineBottom; }
            set { _lineBottom = value; OnPropertyChanged(); }
        }

        ObservableDataSource<Point> _lineA = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源A
        /// </summary>
        public ObservableDataSource<Point> LineA
        {
            get { return _lineA; }
            set { _lineA = value; OnPropertyChanged(); }
        }

        ObservableDataSource<Point> _lineB = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源B
        /// </summary>
        public ObservableDataSource<Point> LineB
        {
            get { return _lineB; }
            set { _lineB = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineC = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源C
        /// </summary>
        public ObservableDataSource<Point> LineC
        {
            get { return _lineC; }
            set { _lineC = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineD = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源D
        /// </summary>
        public ObservableDataSource<Point> LineD
        {
            get { return _lineD; }
            set { _lineD = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineE = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源E
        /// </summary>
        public ObservableDataSource<Point> LineE
        {
            get { return _lineE; }
            set { _lineE = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineF = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源F
        /// </summary>
        public ObservableDataSource<Point> LineF
        {
            get { return _lineF; }
            set { _lineF = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineG = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源G
        /// </summary>
        public ObservableDataSource<Point> LineG
        {
            get { return _lineG; }
            set { _lineG = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineH = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源H
        /// </summary>
        public ObservableDataSource<Point> LineH
        {
            get { return _lineH; }
            set { _lineH = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineI = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源I
        /// </summary>
        public ObservableDataSource<Point> LineI
        {
            get { return _lineI; }
            set { _lineI = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineJ = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源J
        /// </summary>
        public ObservableDataSource<Point> LineJ
        {
            get { return _lineJ; }
            set { _lineJ = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineK = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源K
        /// </summary>
        public ObservableDataSource<Point> LineK
        {
            get { return _lineK; }
            set { _lineK = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineL = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源L
        /// </summary>
        public ObservableDataSource<Point> LineL
        {
            get { return _lineL; }
            set { _lineL = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineM = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源M
        /// </summary>
        public ObservableDataSource<Point> LineM
        {
            get { return _lineM; }
            set { _lineM = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineN = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源N
        /// </summary>
        public ObservableDataSource<Point> LineN
        {
            get { return _lineN; }
            set { _lineN = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineO = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源O
        /// </summary>
        public ObservableDataSource<Point> LineO
        {
            get { return _lineO; }
            set { _lineO = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineJia = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源-甲
        /// </summary>
        public ObservableDataSource<Point> LineJia
        {
            get { return _lineJia; }
            set { _lineJia = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineYi = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源-乙
        /// </summary>
        public ObservableDataSource<Point> LineYi
        {
            get { return _lineYi; }
            set { _lineYi = value; OnPropertyChanged(); }
        }
        ObservableDataSource<Point> _lineBing = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源-丙
        /// </summary>
        public ObservableDataSource<Point> LineBing
        {
            get { return _lineBing; }
            set { _lineBing = value; OnPropertyChanged(); }
        }
                ObservableDataSource<Point> _lineDing = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源-丁
        /// </summary>
        public ObservableDataSource<Point> LineDing
        {
            get { return _lineDing; }
            set { _lineDing = value; OnPropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public SensorTestMultiViewModel(TemperatureTolerance tolerance)
        {
            tol = tolerance;

            StartCommand = new DelegateCommand(Start);
            StopCommand = new DelegateCommand(Stop);
            SaveAndCloseCommand = new DelegateCommand(SaveAndClose); 
            ReturnCommand = new DelegateCommand(Return);

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
                CurrentTemperature = string.IsNullOrEmpty(templist[0].HumidityValue)?
                    templist[0].TemperatureValue + "℃" :
                    templist[0].TemperatureValue + "℃|" + templist[0].HumidityValue + "%";
                TabItemIndex = 0;
            }
        }

        /// <summary>
        /// 实时刷新数据
        /// </summary>
        public void SyncData()
        {
            instrument.Close();

            if (!instrument.Open())
            {
                System.Windows.Forms.MessageBox.Show("串口打开失败！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }

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
                            SensorRealValue real = new SensorRealValue();
                            if (test)
                            {
                                int tt = 50;
                                if (i > 29)
                                {
                                    tt = 30;
                                }

                                real.SensorID = i + 1;
                                real.SensorValue = (float)(tt + (new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000)) * 0.001);
                                real.SensorUnit = "℃";

                                if (i > 29)
                                {
                                    real.SensorUnit = "%";
                                }
                            }
                            else
                            {
                                real = _tempList[i];
                            }

                            //试用版，隐藏传感器无用数据
                            if (i >= 30 && i <= 39 && i != 32)
                            {
                                real.SensorValue = 0;
                                real.SensorUnit = string.Empty;
                            }

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
            if (StartOrPause.Equals("||"))
            {
                pause = true;
                StartOrPause = "▶";
                return;
            }

            if (StartOrPause.Equals("▶") && pause)
            {
                pause = false;
                StartOrPause = "||";
                return;
            }

            ClearChartLineData();
            DispatcherTimer timerChart = new DispatcherTimer();
            timerChart.Interval = TimeSpan.FromMilliseconds(500);
            timerChart.Tick += ChartLineData;
            timerChart.Start();

            DispatcherTimer timerBar = new DispatcherTimer();
            timerBar.Interval = TimeSpan.FromSeconds(1);
            timerBar.Tick += ProcessBarChange;
            currTime = DateTime.Now.AddMinutes(tol.TestTimeSpan);
            timerBar.Start();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(tol.TestSampleInterval);
            timer.Tick += CollectData;
            timer.Start();

            DateTime now = DateTime.Now;
            timeout = false;
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                StartOrPause = "||";
                StopEnable = true;
                SaveEnable = false;

                while (!timeout)
                {
                    BarValue = (int)(DateTime.Now - now).TotalSeconds * 100 / (tol.TestTimeSpan * 60);
                    timeout = BarValue >= 100;
                    Thread.Sleep(500);
                    //暂停
                    while (pause)
                    {
                        Thread.Sleep(300);
                    }
                }

                timerChart.Stop();
                timerBar.Stop();
                timer.Stop();
                StartOrPause = "▶";
                SaveEnable = true;
                MessageBox.Show("设置测量" + CurrentTemperature + " 测量结束");

                if (TestResultDataList.Count > temperatureIndex + 1)
                {
                    temperatureIndex = temperatureIndex + 1;
                    TabItemIndex = TabItemIndex + 1;
                    CurrentTemperature = string.IsNullOrEmpty(TestResultDataList[temperatureIndex].HumidityValue) ?
                    TestResultDataList[temperatureIndex].TemperatureValue + "℃" :
                    TestResultDataList[temperatureIndex].TemperatureValue + "℃|" + TestResultDataList[temperatureIndex].HumidityValue + "%";
                }
            }));
            thrP.IsBackground = true;
            thrP.Start();
        }

        /// <summary>
        /// 实时曲线绘画事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartLineData(object sender, EventArgs e)
        {
            HorizontalDateTimeAxis datetimeAxis = new HorizontalDateTimeAxis();

            LineRight.AppendMany(new List<Point> {
                    new Point(
                        datetimeAxis.ConvertToDouble(currTime.AddSeconds(5)),
                        tol.Info.YTop
                        ) });

            LineTop.AppendMany(new List<Point> {
                    new Point(
                        datetimeAxis.ConvertToDouble(DateTime.Now),
                        tol.Info.YTop
                        ) });

            LineBottom.AppendMany(new List<Point> {
                    new Point(
                        datetimeAxis.ConvertToDouble(DateTime.Now),
                        tol.Info.YBottom
                        ) });

            SetChartLineData(LineA, "A");
            SetChartLineData(LineB, "B");
            SetChartLineData(LineC, "C");
            SetChartLineData(LineD, "D");
            SetChartLineData(LineE, "E");
            SetChartLineData(LineF, "F");
            SetChartLineData(LineG, "G");
            SetChartLineData(LineH, "H");
            SetChartLineData(LineI, "I");
            SetChartLineData(LineJ, "J");
            SetChartLineData(LineK, "K");
            SetChartLineData(LineL, "L");
            SetChartLineData(LineM, "M");
            SetChartLineData(LineN, "N");
            SetChartLineData(LineO, "O");
            SetChartLineData(LineJia, "甲");
            SetChartLineData(LineYi, "乙");
            SetChartLineData(LineBing, "丙");
            SetChartLineData(LineDing, "丁");
        }

        private void SetChartLineData(ObservableDataSource<Point> line, string position)
        {
            if (PositionName.ContainsKey(position))
            {
                HorizontalDateTimeAxis datetimeAxis = new HorizontalDateTimeAxis();

                line.AppendMany(new List<Point> {
                    new Point(
                        datetimeAxis.ConvertToDouble(DateTime.Now),
                        SensorList[PositionName[position].SensorID - 1].SensorValue
                        ) });
            }

        }

        private void ClearChartLineData()
        {
            LineTop.Collection.Clear();
            LineBottom.Collection.Clear();
            LineA.Collection.Clear();
            LineB.Collection.Clear();
            LineC.Collection.Clear();
            LineD.Collection.Clear();
            LineE.Collection.Clear();
            LineF.Collection.Clear();
            LineG.Collection.Clear();
            LineH.Collection.Clear();
            LineI.Collection.Clear();
            LineJ.Collection.Clear();
            LineK.Collection.Clear();
            LineL.Collection.Clear();
            LineM.Collection.Clear();
            LineN.Collection.Clear();
            LineO.Collection.Clear();
            LineJia.Collection.Clear();
            LineYi.Collection.Clear();
            LineBing.Collection.Clear();
            LineDing.Collection.Clear();
        }

        /// <summary>
        /// 进度条变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessBarChange(object sender, EventArgs e)
        {
            if (pause)
            {
                return;
            }

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
            if (pause)
            {
                return;
            }

            TestData data = new TestData
            {
                RecordSN = tol.RecordSN,
                TemperatureName = TestResultDataList[temperatureIndex].TemperatureName,
                TemperatureValue = TestResultDataList[temperatureIndex].TemperatureValue,
                HumidityValue = TestResultDataList[temperatureIndex].HumidityValue,
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Count = TestResultDataList[temperatureIndex].DataList.Count + 1,
                DeviceTemperature = TestResultDataList[temperatureIndex].TemperatureValue,
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

        /// <summary>
        /// 给各个测试点赋值
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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
        /// 保存并关闭窗口
        /// </summary>
        private void SaveAndClose()
        {
            tol.Info.PositionType = tol.PositionType;
            tol.Info.TestTimeSpan = tol.TestTimeSpan;
            tol.Info.TestSampleInterval = tol.TestSampleInterval;
            TestInfoDAO.Save(tol.Info);
            for (int i = 0; i < TestResultDataList.Count; i++)
            {
                TestDataDAO.Save(TestResultDataList[i].DataList);
            }

            SaveEnable = false;
        }

        /// <summary>
        /// 返回主界面
        /// </summary>
        private void Return()
        {
            instrument.Close();

            foreach (Window item in Application.Current.Windows)
            {
                if (item.Title.Equals("测试中心") || item.Title.Equals("测试参数") || item.Title.Equals("偏差、波动度及均匀度测试"))
                {
                    item.Close();
                }
            }
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        private void GetLiveData()
        {
            for (int i = 0; i < TestResultDataList.Count; i++)
            {
                TestDataDAO.Save(TestResultDataList[i].DataList);
            }

            foreach (Window item in Application.Current.Windows)
            {
                if (item.Title.Equals("测试中心") || item.Title.Equals("测试参数") || item.Title.Equals("偏差、波动度及均匀度测试"))
                {
                    item.Close();
                }

            }
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
