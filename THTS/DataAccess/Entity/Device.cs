using System.ComponentModel;
using THTS.MVVM;
using System.ComponentModel.DataAnnotations.Schema;

namespace THTS.DataAccess
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public class Device : INotifyPropertyChanged, ISequencedObject
    {
        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string Manufacture { get; set; }
        public string FactoryNo { get; set; }
        public string CertificateNo { get; set; }
        public int CalibrateResult { get; set; }
        public string CalibrateDate { get; set; }
        public string ExpireDate { get; set; }
        public string Remark { get; set; }
        public int IsDelete { get; set; }

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

        public Device()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);

            this.CalibrateResult = 1;
            this.IsDelete = 0;
        }

        #region
        [NotMapped]
        public string ChannelNo
        {
            get { return SequenceNumber.ToString("00"); }
        }
        #endregion

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
