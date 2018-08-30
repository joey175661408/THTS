using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using THTS.MVVM;

namespace THTS.DataAccess.Entity
{
    public class TestData : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string RecordSN { get; set; }

        public string TemperatureName { get; set; }
        public string TemperatureValue { get; set; }
        public string Time { get; set; }
        public int Count { get; set; }
        public string DeviceTemperature { get; set; }
        public string DeviceHumidity { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string F { get; set; }
        public string G { get; set; }
        public string H { get; set; }
        public string I { get; set; }
        public string J { get; set; }
        public string K { get; set; }
        public string L { get; set; }
        public string M { get; set; }
        public string N { get; set; }
        public string O { get; set; }
        public string Jia { get; set; }
        public string Yi { get; set; }
        public string Bing{ get; set; }
        public string Ding { get; set; }

        public TestData()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        #region NotMapped

        [NotMapped]
        public string TemperatureTitle
        {
            get { return TemperatureName + "(" + TemperatureValue + "℃)  "; }
        }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    /// <summary>
    /// 界面显示数据列表
    /// </summary>
    public class TestDataList : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TemperatureName { get; set; }
        public string TemperatureValue { get; set; }
        public ObservableCollection<TestData> DataList { get; set; }

        public TestDataList()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            DataList = new ObservableCollection<TestData>();
        }

        #region NotMapped

        [NotMapped]
        public string TemperatureTitle
        {
            get { return TemperatureName + "(" + TemperatureValue + "℃)  "; }
        }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
