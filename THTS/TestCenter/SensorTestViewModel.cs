using System;
using System.Collections.Generic;
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


        public SensorTestViewModel()
        {
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
            //GetLiveData();
        }

        /// <summary>
        /// 实时刷新数据
        /// </summary>
        public void SyncData()
        {
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                GetLiveData();
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
    }
}
