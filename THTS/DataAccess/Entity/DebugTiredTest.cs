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
            get
            {
                return new ObservableCollection<SensorRealValue>(
                    JsonHelper.DeserializeJsonToList<SensorRealValue>(SensorValue));
                
            }
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class DebugTiredExport
    {

    }

    public class DebugSensorRealValue : INotifyPropertyChanged, ISequencedObject
    {
        public DateTime Time { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }

        public DebugSensorRealValue()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ISequencedObject 成员
        [NotMapped]
        public int SequenceNumber
        {
            get
            {
                return this.GetBackingValue(() => this.SequenceNumber);
            }
            set
            {
                this.SetBackingValue(() => this.SequenceNumber, value);
            }
        }

        #endregion
    }
}
