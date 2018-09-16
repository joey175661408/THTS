using THTS.MVVM;
using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using THTS.SerialPort;
using THTS.DataAccess;
using System.Windows.Threading;

namespace THTS.DebugCenter
{
    public class TiredTestViewModel : NotifyObject
    {
        #region 命令
        public IDelegateCommand ConnectCommand { get; private set; }
        public IDelegateCommand RefreshTaskNameCommand { get; private set; }
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand SearchCommand { get; private set; }
        public IDelegateCommand ExportCommand { get; private set; }
        public IDelegateCommand CloseCommand { get; private set; }
        #endregion

        #region 属性

        private List<string> _portNameList = new List<string>();
        /// <summary>
        /// 串口号列表
        /// </summary>
        public List<string> PortNameList
        {
            get { return _portNameList; }
            set { _portNameList = value; OnPropertyChanged(); }
        }

        private string _portName = string.Empty;
        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; OnPropertyChanged(); }
        }

        private int _baudRate = 115200;
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; OnPropertyChanged(); }
        }

        private string _connectButtonName = "连接";
        /// <summary>
        /// 连接按钮显示
        /// </summary>
        public string ConnectButtonName
        {
            get { return _connectButtonName; }
            set { _connectButtonName = value; OnPropertyChanged(); }
        }

        private string _deviceType = "离线";
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DeviceType
        {
            get { return _deviceType; }
            set { _deviceType = value; OnPropertyChanged(); }
        }

        private string _taskName = "Task" + DateTime.Now.ToString("yyyyMMddHHmmss");
        /// <summary>
        /// 采集任务名称
        /// </summary>
        public string TaskName
        {
            get { return _taskName; }
            set { _taskName = value; OnPropertyChanged(); }
        }

        private int _logTime = 60;
        /// <summary>
        /// 采集时间
        /// </summary>
        public int LogTime
        {
            get { return _logTime; }
            set { _logTime = value; OnPropertyChanged(); }
        }

        private int _logInterval = 1000;
        /// <summary>
        /// 采集间隔
        /// </summary>
        public int LogInterval
        {
            get { return _logInterval; }
            set { _logInterval = value; OnPropertyChanged(); }
        }

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

        private string _startTime = "0.00";
        /// <summary>
        /// 采集时间倒计时
        /// </summary>
        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; OnPropertyChanged(); }
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

        private ObservableCollection<DebugTiredTest> _sensorSaveList = new ObservableCollection<DebugTiredTest>();
        /// <summary>
        /// 传感器采集的数据
        /// </summary>
        public ObservableCollection<DebugTiredTest> SensorSaveList
        {
            get { return _sensorSaveList; }
            set { _sensorSaveList = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 实时显示数据线程
        /// </summary>
        private bool running = true;


        private List<string> _taskNameList = new List<string>();
        /// <summary>
        /// 所有任务名称列表
        /// </summary>
        public List<string> TaskNameList
        {
            get { return _taskNameList; }
            set { _taskNameList = value; OnPropertyChanged(); }
        }

        private string _searchTaskName = "Task20180510232604";
        /// <summary>
        /// 查询的任务名称
        /// </summary>
        public string SearchTaskName
        {
            get { return _searchTaskName; }
            set { _searchTaskName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DebugSensorRealValue> _selectSearchData = 
            new ObservableCollection<DebugSensorRealValue>();
        /// <summary>
        /// 选中的查询结果数据
        /// </summary>
        public ObservableCollection<DebugSensorRealValue> SelectSearchData
        {
            get { return _selectSearchData; }
            set { _selectSearchData = value; OnPropertyChanged(); }
        }

        private Dictionary<int, ObservableCollection<DebugSensorRealValue>> _searchAllData = 
            new Dictionary<int, ObservableCollection<DebugSensorRealValue>>();
        /// <summary>
        /// 查询的所有数据
        /// </summary>
        public Dictionary<int, ObservableCollection<DebugSensorRealValue>> SearchAllData
        {
            get { return _searchAllData; }
            set { _searchAllData = value; }
        }

        iInstrument instrument = new iInstrument("COM4", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

        #endregion

        #region 构造函数
        public TiredTestViewModel()
        {
            ConnectCommand = new DelegateCommand(Connect);
            RefreshTaskNameCommand = new DelegateCommand(RefreshTaskName);
            StartCommand = new DelegateCommand(Start);
            SearchCommand = new DelegateCommand(Search);
            ExportCommand = new DelegateCommand(Export);
            CloseCommand = new DelegateCommand(CloseView);

            PortNameList = new List<string>(System.IO.Ports.SerialPort.GetPortNames());

            if (PortNameList.Count >= 1)
            {
                PortName = PortNameList[0];
            }

            TaskNameList = DebugTiredTestDAO.GetAllTaskName();

        }
        #endregion

        #region 方法

        /// <summary>
        /// 连接设备
        /// </summary>
        private void Connect()
        {
            if(PortName == string.Empty)
            {
                MessageBox.Show("请选择串口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_connectButtonName.Equals("断开"))
            {
                ConnectButtonName = "连接";
                DeviceType = "离线";
                running = false;

                instrument.Close();
                return;
            }

            ConnectButtonName = "断开";
            running = true;

            instrument = new iInstrument(PortName, BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            if (!instrument.Open())
            {
                return;
            }

            DeviceType = instrument.GetDeviceType();

            if (!DeviceType.Equals(string.Empty))
            {
                Thread thrP = new Thread(new ThreadStart(() =>
                {
                    while (running)
                    {
                        Thread.Sleep(500);
                        ALLSensorValue allSensorValue = new ALLSensorValue();
                        if (instrument.GetALLSensorValue(out allSensorValue))
                        {
                            ObservableCollection<SensorRealValue> _tempList = allSensorValue.SensorList;

                            for (int i = 0; i < _tempList.Count; i++)
                            {
                                #region Test
                                //SensorRealValue real = new SensorRealValue();
                                //real.SensorID = i + 1;
                                //real.SensorValue = (float)(new Random(10).Next() * DateTime.Now.Millisecond);
                                //real.SensorUnit = "℃";
                                #endregion

                                SensorRealValue real = _tempList[i];

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
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        private void Start()
        {
            Search();

            if(DeviceType == "离线")
            {
                MessageBox.Show("设备已离线！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (TaskNameList.Contains(TaskName))
            {
                MessageBox.Show("此任务名称已存在，请尝试修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(LogInterval);
            timer.Tick += CollectData; 
            timer.Start();

            DateTime now = DateTime.Now;
            bool timeout = false;

            Thread thrP = new Thread(new ThreadStart(() =>
            {
                StartEnable = false;

                while (!timeout)
                {
                    Thread.Sleep(300);
                    BarValue = (int)(DateTime.Now - now).TotalSeconds;
                    timeout = BarValue >= (LogTime + LogInterval / 1000);
                }

                timer.Stop();
                StartTime = "100.00";
                DebugTiredTestDAO.Save(SensorSaveList);
                StartEnable = true;
            }));
            thrP.IsBackground = true;
            thrP.Start();


        }

        /// <summary>
        /// 采集数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectData(object sender, EventArgs e)
        {
            DebugTiredTest testData = new DebugTiredTest();
            testData.TaskName = TaskName;
            testData.Time = DateTime.Now;//.ToString("yyyy-MM-dd HH:mm:ss.fff");
            testData.SensorValue = JsonHelper.SerializeObject(SensorList);

            SensorSaveList.Add(testData);
            double temp = BarValue * 100 / LogTime;
            temp = temp > 100 ? 100.00 : temp;
            StartTime = temp.ToString("f2");
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void Search()
        {
            TaskNameList = DebugTiredTestDAO.GetAllTaskName();
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        private void Export()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "xls files(*.xls)|*.xls|All files(*.*)|*.*";
            save.DefaultExt = "xls";
            save.AddExtension = true;
            save.RestoreDirectory = true;
            save.FileName = SearchTaskName + ".xls";
            if(save.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ObservableCollection<DebugTiredTest> allData = DebugTiredTestDAO.GetAllData(SearchTaskName);

                    ExcelHelper helper = new ExcelHelper();
                    helper.InitializeWorkbook();

                    string[] columns = new string[42];
                    columns[0] = "序号";
                    columns[1] = "时间";

                    for (int i = 1; i <= 40; i++)
                    {
                        columns[i + 1] = "通道" + i;
                    }


                    helper.SetTitleValue(0, columns, SearchTaskName);
                    helper.SetTiredValue(allData, 1, SearchTaskName);
                    helper.WriteToFile(save.FileName, false);
                    MessageBox.Show("导出成功！","提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出失败！\r\n" + ex.Message);
                }
                
            }
        }

        /// <summary>
        /// 刷新任务名称
        /// </summary>
        private void RefreshTaskName()
        {
            TaskName = "Task" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void CloseView()
        {
            instrument.Close();

            foreach (System.Windows.Window item in System.Windows.Application.Current.Windows)
            {
                if (item.Title.Equals("疲劳性测试"))
                {
                    item.Close();
                }
            }
        }

        #endregion
    }
}
