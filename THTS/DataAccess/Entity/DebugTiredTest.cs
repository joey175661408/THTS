using System;
using System.ComponentModel;
using THTS.MVVM;
using THTS.SerialPort;
using System.Collections.ObjectModel;

namespace THTS.DataAccess
{
    public class DebugTiredTest : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public ObservableCollection<SensorRealValue> SensorValue { get; set; }

        public DebugTiredTest()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }


        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
