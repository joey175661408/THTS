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

namespace THTS.TestCenter
{
    public class SensorTestViewModel : NotifyObject
    {
        #region Command
        /// <summary>
        /// 压力量程或温度范围单位切换命令
        /// </summary>
        public IDelegateCommand SelectedPositionCommand { get; private set; }
        #endregion

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

        /// <summary>
        /// 通讯实例
        /// </summary>
        iInstrument instrument = new iInstrument("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);


        /// <summary>
        /// 构造函数
        /// </summary>
        public SensorTestViewModel()
        {
            _testPositionList = new List<string>();
            _testPositionList.Add("9测点分布体");
            _testPositionList.Add("15测点分布体");
            SelectedTestPosition = _testPositionList[0];

            for (int i = 1; i <= 40; i++)
            {
                _sensorIDList.Add(i.ToString());
            }

            SelectedPositionCommand = new DelegateCommand(TestPositionChanged);

            //获取串口配置信息
            DataAccess.Setting settings = DataAccess.SettingsDAO.GetData();

            instrument = new iInstrument(settings.PortName, settings.BaudRate, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);

            SyncData();
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
                            real.SensorValue = (float)(new Random(10).Next() * DateTime.Now.Millisecond);
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
        /// 获取实时数据
        /// </summary>
        private void GetLiveData()
        {
            iInstrument instrument = new iInstrument("COM1", 38400, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.Two);

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
    }
}
