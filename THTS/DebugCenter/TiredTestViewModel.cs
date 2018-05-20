using THTS.MVVM;
using System;
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
        public IDelegateCommand StartCommand { get; private set; }
        public IDelegateCommand SearchCommand { get; private set; }
        public IDelegateCommand SelectSensorCommand { get; private set; }
        public IDelegateCommand ExportCommand { get; private set; }
        #endregion

        #region 属性

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


        private string _searchTaskName = "Task20180510232604";
        /// <summary>
        /// 查询的任务名称
        /// </summary>
        public string SearchTaskName
        {
            get { return _searchTaskName; }
            set { _searchTaskName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _sensorIDList = new ObservableCollection<int>();
        /// <summary>
        /// 传感器ID查询条件
        /// </summary>
        public ObservableCollection<int> SensorIDList
        {
            get { return _sensorIDList; }
            set { _sensorIDList = value; OnPropertyChanged(); }
        }

        private int _selectSensorID = -1;
        /// <summary>
        /// 选中的传感器ID
        /// </summary>
        public int SelectSensorID
        {
            get { return _selectSensorID; }
            set { _selectSensorID = value; OnPropertyChanged(); }
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
            StartCommand = new DelegateCommand(Start);
            SearchCommand = new DelegateCommand(Search);
            SelectSensorCommand = new DelegateCommand(SelectSensor);
            ExportCommand = new DelegateCommand(Export);

            for (int i = 1; i <= 40; i++)
            {
                SensorIDList.Add(i);
            }
        }
        #endregion

        #region 方法

        /// <summary>
        /// 连接设备
        /// </summary>
        private void Connect()
        {
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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(LogInterval);
            timer.Tick += CollectData; 
            timer.Start();

            DateTime now = DateTime.Now;
            bool timeout = false;

            Thread thrP = new Thread(new ThreadStart(() =>
            {
                while (!timeout)
                {
                    Thread.Sleep(300);
                    timeout = (DateTime.Now - now).TotalSeconds >= (LogTime + LogInterval / 1000);
                }

                timer.Stop();

                DebugTiredTestDAO.Save(SensorSaveList);
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
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        private void Search()
        {
            ObservableCollection<DebugTiredTest> allData = DebugTiredTestDAO.GetAllData(SearchTaskName);

            for (int i = 0; i < allData.Count; i++)
            {
                ObservableCollection<DebugSensorRealValue> tempData = new ObservableCollection<DebugSensorRealValue>();
                for (int j = 0; j < allData[i].SensorValueList.Count; j++)
                {
                    DebugSensorRealValue temp = new DebugSensorRealValue();
                    temp.Time = allData[i].Time;
                    temp.Value = allData[i].SensorValueList[j].SensorValue;
                    temp.Unit = allData[i].SensorValueList[j].SensorUnit;
                    tempData.Add(temp);
                }

                if (SearchAllData.ContainsKey(allData[i].Id))
                {
                    SearchAllData[allData[i].Id] = tempData;
                }
                else
                {
                    SearchAllData.Add(allData[i].Id, tempData);
                }
            }

            SelectSensorID = 1;
        }

        /// <summary>
        /// 选择查询条件-传感器ID
        /// </summary>
        private void SelectSensor()
        {
            if (SearchAllData.ContainsKey(SelectSensorID))
            {
                SelectSearchData = SearchAllData[SelectSensorID];
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        private void Export()
        {

        }

        #endregion
    }
}
