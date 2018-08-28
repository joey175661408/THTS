using THTS.MVVM;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using THTS.DataModule;

namespace THTS.DataAccess
{
    /// <summary>
    /// 温度偏差、波动度及均匀度测试参数
    /// </summary>
    public class TemperatureTolerance : INotifyPropertyChanged
    {
        #region 测试温度参数
        public int Id { get; set;}

        public int PositionType { get; set; }

        public int TestTimeSpan { get; set; }
        public int TestSampleInterval { get; set; }

        #endregion

        public TemperatureTolerance()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            TemperatureList = new ObservableCollection<TestTemperatureModule>();
            PositionList = new Dictionary<string, Sensor>();
        }

        #region NotMapped

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
