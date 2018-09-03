﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.Charts;
using THTS.DataAccess;

namespace THTS.TestCenter
{
    public partial class SensorTest
    {

        public SensorTest(TemperatureTolerance tolerance)
        {
            InitializeComponent();
            this.DataContext = new SensorTestViewModel(tolerance);

            //this.lineTemperature.DataSource = TemperatureCollections;
        }

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
    }

    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
            typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
