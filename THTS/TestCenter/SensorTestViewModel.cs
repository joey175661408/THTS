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

namespace THTS.TestCenter
{
    public class SensorTestViewModel : NotifyObject
    {
        #region Command

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

        private Dictionary<string, Sensor> _positionName;
        /// <summary>
        /// 测点分布与传感器对应关系
        /// </summary>
        public Dictionary<string, Sensor> PositionName
        {
            get { return _positionName; }
            set { _positionName = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TestData> _testDataList = new ObservableCollection<TestData>();
        /// <summary>
        /// 数据采集结果
        /// </summary>
        public ObservableCollection<TestData> TestDataList
        {
            get { return _testDataList; }
            set { _testDataList = value; OnPropertyChanged(); }
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
                TestData data = new TestData();
                data.TemperatureName = templist[i].TestTemperatureID;
                data.TemperatureValue = templist[i].TemperatureValue;
                TestDataList.Add(data);
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
            iInstrument instrument = new iInstrument("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.Two);

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
