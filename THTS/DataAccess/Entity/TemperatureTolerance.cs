using THTS.MVVM;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace THTS.DataAccess
{
    /// <summary>
    /// 温度偏差、波动度及均匀度测试参数
    /// </summary>
    public class TemperatureTolerance : INotifyPropertyChanged
    {
        #region 测试温度参数
        public int Id { get; set;}

        public ObservableCollection<double> TemperatureList { get; set; }

        public ObservableCollection<Sensor> SensorList { get; set; }

        public int PositionType { get; set; }

        public string TestTimeSpan { get; set; }
        public string TestSampleInterval { get; set; }

        #endregion

        public TemperatureTolerance()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            TemperatureList = new ObservableCollection<double>();
            SensorList = new ObservableCollection<Sensor>();
        }

        #region NotMapped

        
        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
