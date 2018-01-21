using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts;

namespace THTS.TestCenter
{
    public partial class SensorTest
    {
        ObservableDataSource<Point> _TemperatureCollections = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源
        /// </summary>
        public ObservableDataSource<Point> TemperatureCollections
        {
            get { return _TemperatureCollections; }
            set { _TemperatureCollections = value;}
        }


        public SensorTest()
        {
            InitializeComponent();
            this.lineTemperature.DataSource = TemperatureCollections;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        private void GetLiveData()
        {
            this.Dispatcher.BeginInvoke((ThreadStart)delegate ()
            {
                TemperatureCollections.Collection.Clear();
            });

            HorizontalDateTimeAxis datetimeAxis = new HorizontalDateTimeAxis();

            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    this.Dispatcher.BeginInvoke((ThreadStart)delegate ()
                    {
                        TemperatureCollections.AppendMany(new List<Point> { new Point(datetimeAxis.ConvertToDouble(DateTime.Now), DateTime.Now.Second) });
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }

        }

        private void Chart_Click(object sender, RoutedEventArgs e)
        {
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                GetLiveData();
            }));
            thrP.IsBackground = true;
            thrP.SetApartmentState(ApartmentState.STA);
            thrP.Start();
        }
    }
}
