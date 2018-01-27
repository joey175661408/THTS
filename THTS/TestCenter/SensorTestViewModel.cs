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

namespace THTS.TestCenter
{
    public class SensorTestViewModel : NotifyObject
    {
        ObservableDataSource<Point> _TemperatureCollections = new ObservableDataSource<Point>();
        /// <summary>
        /// 温度曲线数据源
        /// </summary>
        public ObservableDataSource<Point> TemperatureCollections
        {
            get { return _TemperatureCollections; }
            set { _TemperatureCollections = value; OnPropertyChanged(); }
        }

        public void SyncData()
        {
            Thread thrP = new Thread(new ThreadStart(() =>
            {
                GetLiveData();
            }));
            thrP.IsBackground = true;
            thrP.SetApartmentState(ApartmentState.STA);
            thrP.Start();
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        private void GetLiveData()
        {

            HorizontalDateTimeAxis datetimeAxis = new HorizontalDateTimeAxis();

            //bool isStable = false;
            DateTime now = DateTime.Now;
        }



    }
}
