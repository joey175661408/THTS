using THTS.MVVM;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using THTS.DataModule;
using System.Windows;

namespace THTS.DataAccess
{
    /// <summary>
    /// 温度偏差、波动度及均匀度测试参数
    /// </summary>
    public class TemperatureTolerance : INotifyPropertyChanged
    {
        #region 测试温度参数
        public int Id { get; set;}
        public string RecordSN { get; set; }

        public int PositionType { get; set; }

        public int TestTimeSpan { get; set; }
        public int TestSampleInterval { get; set; }

        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public string Extra6 { get; set; }
        public string Extra7 { get; set; }
        public string Extra8 { get; set; }
        public string Extra9 { get; set; }
        public string Extra10 { get; set; }

        #endregion

        public TemperatureTolerance()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            TemperatureList = new ObservableCollection<TestTemperatureModule>();
            PositionList = new Dictionary<string, Sensor>();
        }

        #region NotMapped

        [NotMapped]
        public TestInfo Info { get; set; }

        [NotMapped]
        public Visibility UC5Visibility
        {
            get
            {
                return PositionType == 5 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [NotMapped]
        public Visibility UC9Visibility
        {
            get
            {
                return PositionType == 9 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [NotMapped]
        public Visibility UC15Visibility
        {
            get
            {
                return PositionType == 15 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility UC27Visibility
        {
            get
            {
                return PositionType == 27 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [NotMapped]
        public Visibility Humidity5Visibility
        {
            get
            {
                return PositionType == 5 && Info.HumidityVisibility == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [NotMapped]
        public Visibility Humidity9Visibility
        {
            get
            {
                return PositionType == 9 && Info.HumidityVisibility == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility Humidity15Visibility
        {
            get
            {
                return PositionType == 15 && Info.HumidityVisibility == Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        [NotMapped]
        public Dictionary<string,Sensor> PositionList { get; set; }

        [NotMapped]
        public ObservableCollection<TestTemperatureModule> TemperatureList { get; set; }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
