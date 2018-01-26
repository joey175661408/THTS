using THTS.MVVM;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace THTS.DataAccess
{
    /// <summary>
    /// 温度偏差、波动度及均匀度测试参数
    /// </summary>
    public class TemperatureTolerance : INotifyPropertyChanged
    {
        #region 测试温度参数
        public int Id { get; set;}

        public int OnLower { get; set; }
        public string TemperatureLower { get; set; }
        public string WaitLower { get; set; }
        public string ToleranceLower { get; set; }
        public int OnUpper { get; set; }
        public string TemperatureUpper { get; set; }
        public string WaitUpper { get; set; }
        public string ToleranceUpper { get; set; }
        public int OnTest1 { get; set; }
        public string TemperatureTest1 { get; set; }
        public string WaitTest1 { get; set; }
        public string ToleranceTest1 { get; set; }
        public int OnTest2 { get; set; }
        public string TemperatureTest2 { get; set; }
        public string WaitTest2 { get; set; }
        public string ToleranceTest2 { get; set; }
        public int OnTest3 { get; set; }
        public string TemperatureTest3 { get; set; }
        public string WaitTest3 { get; set; }
        public string ToleranceTest3 { get; set; }
        public int OnTest4 { get; set; }
        public string TemperatureTest4 { get; set; }
        public string WaitTest4 { get; set; }
        public string ToleranceTest4 { get; set; }
        public int OnTest5 { get; set; }
        public string TemperatureTest5 { get; set; }
        public string WaitTest5 { get; set; }
        public string ToleranceTest5 { get; set; }
        public int OnTest6 { get; set; }
        public string TemperatureTest6 { get; set; }
        public string WaitTest6 { get; set; }
        public string TeranceTest6 { get; set; }
        public int OnTest7 { get; set; }
        public string TemperatureTest7 { get; set; }
        public string WaitTest7 { get; set; }
        public string ToleranceTest7 { get; set; }
        public int OnTest8 { get; set; }
        public string TemperatureTest8 { get; set; }
        public string WaitTest8 { get; set; }
        public string ToleranceTest8 { get; set; }
        public int OnTest9 { get; set; }
        public string TemperatureTest9 { get; set; }
        public string WaitTest9 { get; set; }
        public string ToleranceTest9 { get; set; }
        public int OnTest10 { get; set; }
        public string TemperatureTest10 { get; set; }
        public string WaitTest10 { get; set; }
        public string ToleranceTest10 { get; set; }

        public string Atmosphere { get; set; }
        public string WindSpeed { get; set; }

        public string Channel { get; set; }
        public string SensorIndex { get; set; }
        public string SensorName { get; set; }
        public string SensorSN { get; set; }

        public int IsCJC { get; set; }
        public string ChannelCJC { get; set; }
        public int TypeCJC { get; set; }
        public string TempertureCJC { get; set; }

        public string TestTimeSpan { get; set; }
        public string TestSampleInterval { get; set; }

        public int ContainMedium { get; set; }
        public string MediumDescription { get; set; }
        public int ContainLoad { get; set; }
        public string LoadDescription { get; set; }
        #endregion

        public TemperatureTolerance()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        #region NotMapped

        [NotMapped]
        public ObservableCollection<DataAccess.Device> DeviceSelectList { get; set; }

        [NotMapped]
        public bool? OnLowerIsChecked
        {
            get { return OnLower == 1; }
            set { OnLower = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnUpperIsChecked
        {
            get { return OnUpper == 1; }
            set { OnUpper = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest1IsChecked
        {
            get { return OnTest1 == 1; }
            set { OnTest1 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest2IsChecked
        {
            get { return OnTest2 == 1; }
            set { OnTest2 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest3IsChecked
        {
            get { return OnTest3 == 1; }
            set { OnTest3 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest4IsChecked
        {
            get { return OnTest4 == 1; }
            set { OnTest4 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest5IsChecked
        {
            get { return OnTest5 == 1; }
            set { OnTest5 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest6IsChecked
        {
            get { return OnTest6 == 1; }
            set { OnTest6 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest7IsChecked
        {
            get { return OnTest7 == 1; }
            set { OnTest7 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest8IsChecked
        {
            get { return OnTest8 == 1; }
            set { OnTest8 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest9IsChecked
        {
            get { return OnTest9 == 1; }
            set { OnTest9 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? OnTest10IsChecked
        {
            get { return OnTest10 == 1; }
            set { OnTest10 = value.HasValue && value.Value ? 1 : 0; }
        }

        [NotMapped]
        public bool? IsCJCIsChecked
        {
            get
            {
                return IsCJC == 1;
            }

            set
            {
                IsCJC = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? TypeCJCIsAuto
        {
            get
            {
                return TypeCJC == 1;
            }

            set
            {
                TypeCJC = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? ContainMediumIsChecked
        {
            get
            {
                return ContainMedium == 1;
            }

            set
            {
                ContainMedium = value.HasValue && value.Value ? 1 : 0;
            }
        }

        [NotMapped]
        public bool? ContainLoadIsChecked
        {
            get
            {
                return ContainLoad == 1;
            }

            set
            {
                ContainLoad = value.HasValue && value.Value ? 1 : 0;
            }
        }
        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
