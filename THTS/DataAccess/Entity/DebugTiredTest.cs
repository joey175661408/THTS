using System;
using System.ComponentModel;
using THTS.MVVM;
using THTS.SerialPort;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace THTS.DataAccess
{
    public class DebugTiredTest : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime Time { get; set; }
        public string SensorValue { get; set; }

        public DebugTiredTest()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        [NotMapped]
        public ObservableCollection<SensorRealValue> SensorValueList
        {
            get { return new ObservableCollection<SensorRealValue>(); }
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
