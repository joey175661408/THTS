using System.ComponentModel;
using THTS.MVVM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace THTS.DataAccess
{
    public class Setting : INotifyPropertyChanged, ISequencedObject
    {
        [Key]
        public string No { get; set; }
        public string PortName { get; set; }
        public int BaudRate { get; set; }

        public Setting()
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
