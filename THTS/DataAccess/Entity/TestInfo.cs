using System.ComponentModel;
using THTS.MVVM;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
using System;
using THTS.DataAccess.EntityDAO;

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
        public int PositionType { get; set; }
        public int TestTimeSpan { get; set; }
        public int TestSampleInterval { get; set; }
        public int IsDeleted { get; set; }
        /// <summary>
        /// 下限湿度
        /// </summary>
        public string Extra1 { get; set; }
        /// <summary>
        /// 上限湿度
        /// </summary>
        public string Extra2 { get; set; }
        /// <summary>
        /// 湿度精确度
        /// </summary>
        public string Extra3 { get; set; }
        /// <summary>
        /// PT100温度传感器是否修正(True/False)
        /// </summary>
        public string Extra4 { get; set; }
        /// <summary>
        /// 湿度传感器是否修正(True/False)
        /// </summary>
        public string Extra5 { get; set; }
        /// <summary>
        /// K型热电偶温度传感器是否修正(True/False)
        /// </summary>
        public string Extra6 { get; set; }
        public string Extra7 { get; set; }
        public string Extra8 { get; set; }
        public string Extra9 { get; set; }
        public string Extra10 { get; set; }

        //标准设备信息
        public string CalName { get; set; }
        public string CalModule { get; set; }
        public string CalAccuracy { get; set; }
        public string CalCertificateSN { get; set; }
        public string CalExpiryDate { get; set; }

        public TestInfo()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            this.IsDeleted = 0;
        }

        #region NotMapped

        [NotMapped]
        public DateTime GetTestDate
        {
            get { return DateTime.Parse(TestDate); }
        }

        [NotMapped]
        public bool? PT100IsFix
        {
            get
            {
                return Extra4.Equals("True");
            }

            set
            {
                Extra4 = value.HasValue && value.Value ? "True" : "False";
            }
        }

        [NotMapped]
        public bool? HumidityIsFix
        {
            get
            {
                return Extra5.Equals("True");
            }

            set
            {
                Extra5 = value.HasValue && value.Value ? "True" : "False";
            }
        }

        [NotMapped]
        public bool? KtempIsFix
        {
            get
            {
                return Extra6.Equals("True");
            }

            set
            {
                Extra6 = value.HasValue && value.Value ? "True" : "False";
            }
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

        [NotMapped]
        public Visibility TemperatuerVisibility
        {
            get
            {
                if(TemperatureAverage == 0 && TemperatureDeparture == 0)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        [NotMapped]
        public Visibility HumidityVisibility
        {
            get
            {
                if (HumidityAverage == 0 && HumidityDeparture == 0)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 曲线图纵坐标最大值
        /// </summary>
        [NotMapped]
        public double YTop
        {
            get
            {
                try
                {
                    double p = double.Parse(TemperatureUpper);
                    double h = double.Parse(Extra2);

                    if (p > h)
                    {
                        return p;
                    }
                    else
                    {
                        return h;
                    }
                }
                catch (System.Exception)
                {

                    return 105;
                }
            }
        }

        /// <summary>
        /// 曲线图纵坐标最小值
        /// </summary>
        [NotMapped]
        public double YBottom
        {
            get
            {
                try
                {
                    double p = double.Parse(TemperatureLower);
                    double h = double.Parse(Extra1);

                    if (p > h)
                    {
                        return h;
                    }
                    else
                    {
                        return p;
                    }
                }
                catch (System.Exception)
                {

                    return -5;
                }
            }
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

    /// <summary>
    /// 测点位置与传感器对应关系
    /// </summary>
    public class PositionAndSensor : INotifyPropertyChanged, ISequencedObject
    {
        public int Id { get; set; }
        public string RecordSN { get; set; }
        public string PositionName { get; set; }
        public string SensorID { get; set; }
        public int IsDeleted { get; set; }

        public PositionAndSensor()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            this.IsDeleted = 0;
        }

        public PositionAndSensor(string rsn, string pname, string sid)
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            this.RecordSN = rsn;
            this.PositionName = pname;
            this.SensorID = sid;
            this.IsDeleted = 0;
            this.sensor = DeviceDAO.GetDevice(sid);
        }

        [NotMapped]
        public Device sensor { get; set; }

        public void ReflashSensorInfo()
        {
            this.sensor = DeviceDAO.GetDevice(SensorID);
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
