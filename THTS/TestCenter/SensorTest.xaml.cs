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
            set { _TemperatureCollections = value; }
        }

        /// <summary>
        /// 停止采样
        /// </summary>
        private bool Stop = false;

        public SensorTest()
        {
            InitializeComponent();
            this.DataContext = new SensorTestViewModel();

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

            while (!Stop)
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

        /// <summary>
        /// 测点分布和实时曲线切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Visiable_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnChart.Content.Equals("测点分布"))
            {
                this.btnChart.Content = "实时曲线";
                this.gbPoint.Visibility = Visibility.Visible;
                this.gbPointParams.Visibility = Visibility.Visible;
                this.gbChart.Visibility = Visibility.Hidden;
                this.gbChartParams.Visibility = Visibility.Hidden;
            }
            else
            {
                this.btnChart.Content = "测点分布";
                this.gbPoint.Visibility = Visibility.Hidden;
                this.gbPointParams.Visibility = Visibility.Hidden;
                this.gbChart.Visibility = Visibility.Visible;
                this.gbChartParams.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// 开始采样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_Click(object sender, RoutedEventArgs e)
        {
            Stop = false;

            Thread thrP = new Thread(new ThreadStart(() =>
            {
                GetLiveData();
            }));
            thrP.IsBackground = true;
            thrP.SetApartmentState(ApartmentState.STA);
            thrP.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Stop = true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
