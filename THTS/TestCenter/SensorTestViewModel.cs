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

        private ObservableDataSource<SensorRealValue> _channel1List = new ObservableDataSource<SensorRealValue>();
        /// <summary>
        /// 通道1实时数据
        /// </summary>
        public ObservableDataSource<SensorRealValue> Channel1List
        {
            get { return _channel1List; }
            set { _channel1List = value; OnPropertyChanged(); }
        }

        private ObservableDataSource<SensorRealValue> _channel2List = new ObservableDataSource<SensorRealValue>();
        /// <summary>
        /// 通道2实时数据
        /// </summary>
        public ObservableDataSource<SensorRealValue> Channel2List
        {
            get { return _channel2List; }
            set { _channel2List = value; OnPropertyChanged(); }
        }

        private ObservableDataSource<SensorRealValue> _channel3List = new ObservableDataSource<SensorRealValue>();
        /// <summary>
        /// 通道3实时数据
        /// </summary>
        public ObservableDataSource<SensorRealValue> Channel3List
        {
            get { return _channel3List; }
            set { _channel3List = value; OnPropertyChanged(); }
        }

        private ObservableDataSource<SensorRealValue> _channel4List = new ObservableDataSource<SensorRealValue>();
        /// <summary>
        /// 通道4实时数据
        /// </summary>
        public ObservableDataSource<SensorRealValue> Channel4List
        {
            get { return _channel4List; }
            set { _channel4List = value; OnPropertyChanged(); }
        }

        ObservableDataSource<Point> _TemperatureCollections = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源
        /// </summary>
        public ObservableDataSource<Point> TemperatureCollections
        {
            get { return _TemperatureCollections; }
            set { _TemperatureCollections = value; OnPropertyChanged(); }
        }

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

            for (int i = 0; i < 10; i++)
            {
                SensorRealValue sensor1 = new SensorRealValue();
                sensor1.SensorID = i + 1;
                //sensor1.SensorValue = value.Channel1[i].ValueAndUnit;
                //sensor1.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                Channel1List.Collection.Add(sensor1);

                SensorRealValue sensor2 = new SensorRealValue();
                sensor2.SensorID = i + 11;
                //sensor2.SensorValue = value.Channel2[i].ValueAndUnit;
                //sensor1.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                Channel2List.Collection.Add(sensor2);

                SensorRealValue sensor3 = new SensorRealValue();
                sensor3.SensorID = i + 21;
                //sensor3.SensorValue = value.Channel3[i].ValueAndUnit;
                //sensor3.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                Channel3List.Collection.Add(sensor3);

                SensorRealValue sensor4 = new SensorRealValue();
                sensor4.SensorID = i + 31;
                //sensor4.SensorValue = value.Channel4[i].ValueAndUnit;
                //sensor4.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                Channel4List.Collection.Add(sensor4);
            }

            SyncData();
        }

        /// <summary>
        /// 实时刷新数据
        /// </summary>
        public void SyncData()
        {
            iInstrument instrument = new iInstrument("COM1", 38400, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.Two);

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

            ChannelValue value = new ChannelValue();
            while (true)
            {
                Thread.Sleep(500);
                if (instrument.GetSensorValue(out value))
                {
                    Channel1List.Collection.Clear();
                    Channel2List.Collection.Clear();
                    Channel3List.Collection.Clear();
                    Channel4List.Collection.Clear();

                    for (int i = 0; i < 10; i++)
                    {
                        SensorRealValue sensor1 = new SensorRealValue();
                        sensor1.SensorID = i + 1;
                        //sensor1.SensorValue = value.Channel1[i].ValueAndUnit;
                        //sensor1.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                        Channel1List.Collection.Add(sensor1);

                        SensorRealValue sensor2 = new SensorRealValue();
                        sensor2.SensorID = i + 11;
                        //sensor2.SensorValue = value.Channel2[i].ValueAndUnit;
                        //sensor1.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                        Channel2List.Collection.Add(sensor2);

                        SensorRealValue sensor3 = new SensorRealValue();
                        sensor3.SensorID = i + 21;
                        //sensor3.SensorValue = value.Channel3[i].ValueAndUnit;
                        //sensor3.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                        Channel3List.Collection.Add(sensor3);

                        SensorRealValue sensor4 = new SensorRealValue();
                        sensor4.SensorID = i + 31;
                        //sensor4.SensorValue = value.Channel4[i].ValueAndUnit;
                        //sensor4.ValueAndUnit = new Random().NextDouble().ToString() + "℃";
                        Channel4List.Collection.Add(sensor4);
                    }
                }
            }
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
