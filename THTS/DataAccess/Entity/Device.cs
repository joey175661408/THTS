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

        public Device()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);

            this.CalibrateResult = 1;
            this.IsDelete = 0;
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
