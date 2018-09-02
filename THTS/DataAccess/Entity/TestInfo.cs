using System.ComponentModel;
using THTS.MVVM;
using System.ComponentModel.DataAnnotations.Schema;

namespace THTS.DataAccess
{
    /// <summary>
    /// 基本测试参数
    /// </summary>
    public class TestInfo : INotifyPropertyChanged, ISequencedObject
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string UutName { get; set; }
        public string UutModule { get; set; }
        public string UutSN { get; set; }
        public string UutManufacture { get; set; }
        public string UutCustomSN { get; set; }
        public string UutCalPosition { get; set; }
        public string Accuracy { get; set; }
        public string TemperatureLower { get; set; }
        public string TemperatureUpper { get; set; }
        public string EnvironmentTemperature { get; set; }
        public string EnvironmentPressure { get; set; }
        public string EnvironmentHumidity { get; set; }
        public string RecordSN { get; set; }
        public string CertificateSN { get; set; }
        public string VerifiedBy { get; set; }
        public string CheckedBy { get; set; }
        public string TestDate { get; set; }
        public int TemperatureDeparture { get; set; }
        public int TemperatureAverage { get; set; }
        public int HumidityDeparture { get; set; }
        public int HumidityAverage { get; set; }
        public int IsDeleted { get; set; }
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

        public TestInfo()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            this.HumidityDeparture = 0;
            this.HumidityAverage = 0;
            this.IsDeleted = 0;
        }

        [NotMapped]
        public bool? TempDepartureIsChecked
        {
            get
            {
                return TemperatureDeparture == 1;
            }

            set
            {
                TemperatureDeparture = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? TempAverageIsChecked
        {
            get
            {
                return TemperatureAverage == 1;
            }

            set
            {
                TemperatureAverage = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? HumiDepartureIsChecked
        {
            get
            {
                return HumidityDeparture == 1;
            }

            set
            {
                HumidityDeparture = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? HumiAverageIsChecked
        {
            get
            {
                return HumidityAverage == 1;
            }

            set
            {
                HumidityAverage = value.HasValue && value.Value ? 1 : 0;
            }
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
