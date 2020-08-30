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
        public string NameCH { get; set; }
        public string NameEN { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Supervisory { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModule { get; set; }
        public string DeviceAccuracy { get; set; }
        public string DeviceCertificate { get; set; }
        public string CalStandard { get; set; }

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

        public Setting()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            this.CalStandard = "JJF 1101 - 2019 环境试验设备温度、湿度校准规范";
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
