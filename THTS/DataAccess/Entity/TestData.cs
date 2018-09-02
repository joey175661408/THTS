using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using THTS.MVVM;

namespace THTS.DataAccess.Entity
{
    public class TestData : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string RecordSN { get; set; }

        public string TemperatureName { get; set; }
        public string TemperatureValue { get; set; }
        public string HumidityValue { get; set; }
        public string Time { get; set; }
        public int Count { get; set; }
        public string DeviceTemperature { get; set; }
        public string DeviceHumidity { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }
        public float E { get; set; }
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }
        public float L { get; set; }
        public float M { get; set; }
        public float N { get; set; }
        public float O { get; set; }

        public float Jia { get; set; }
        public float Yi { get; set; }
        public float Bing{ get; set; }
        public float Ding { get; set; }

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

        public TestData()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
        }

        #region NotMapped

        [NotMapped]
        public string TemperatureTitle
        {
            get { return TemperatureName + "(" + TemperatureValue + "℃)  "; }
        }

        [NotMapped]
        public string StringO { get { return O.ToString("0.00"); } }
        [NotMapped]
        public string StringA { get { return A.Equals(-1000) ? "" : A.ToString("0.00"); } }
        [NotMapped]
        public string StringB { get { return B.Equals(-1000) ? "" : B.ToString("0.00"); } }
        [NotMapped]
        public string StringC { get { return C.Equals(-1000) ? "" : C.ToString("0.00"); } }
        [NotMapped]
        public string StringD { get { return D.Equals(-1000) ? "" : D.ToString("0.00"); } }
        [NotMapped]
        public string StringE { get { return E.Equals(-1000) ? "" : E.ToString("0.00"); } }
        [NotMapped]
        public string StringF { get { return F.Equals(-1000) ? "" : F.ToString("0.00"); } }
        [NotMapped]
        public string StringG { get { return G.Equals(-1000) ? "" : G.ToString("0.00"); } }
        [NotMapped]
        public string StringH { get { return H.Equals(-1000) ? "" : H.ToString("0.00"); } }
        [NotMapped]
        public string StringI { get { return I.Equals(-1000) ? "" : I.ToString("0.00"); } }
        [NotMapped]
        public string StringJ { get { return J.Equals(-1000) ? "" : J.ToString("0.00"); } }
        [NotMapped]
        public string StringK { get { return K.Equals(-1000) ? "" : K.ToString("0.00"); } }
        [NotMapped]
        public string StringL { get { return L.Equals(-1000) ? "" : L.ToString("0.00"); } }
        [NotMapped]
        public string StringM { get { return M.Equals(-1000) ? "" : M.ToString("0.00"); } }
        [NotMapped]
        public string StringN { get { return N.Equals(-1000) ? "" : N.ToString("0.00"); } }
        [NotMapped]
        public string StringJia { get { return Jia.Equals(-1000) ? "" : Jia.ToString("0.00"); } }
        [NotMapped]
        public string StringYi { get { return Yi.Equals(-1000) ? "" : Yi.ToString("0.00"); } }
        [NotMapped]
        public string StringBing { get { return Bing.Equals(-1000) ? "" : Bing.ToString("0.00"); } }
        [NotMapped]
        public string StringDing { get { return Ding.Equals(-1000) ? "" : Ding.ToString("0.00"); } }

        [NotMapped]
        public float MaxT
        {
            get
            {
                float temp = O;
                temp = A == -1000 || A < temp ? temp : A;
                temp = B == -1000 || B < temp ? temp : B;
                temp = C == -1000 || C < temp ? temp : C;
                temp = D == -1000 || D < temp ? temp : D;
                temp = E == -1000 || E < temp ? temp : E;
                temp = F == -1000 || F < temp ? temp : F;
                temp = G == -1000 || G < temp ? temp : G;
                temp = H == -1000 || H < temp ? temp : H;
                temp = I == -1000 || I < temp ? temp : I;
                temp = J == -1000 || J < temp ? temp : J;
                temp = K == -1000 || K < temp ? temp : K;
                temp = L == -1000 || L < temp ? temp : L;
                temp = M == -1000 || M < temp ? temp : M;
                temp = N == -1000 || N < temp ? temp : N;
                return temp;
            }
        }

        [NotMapped]
        public float MinT
        {
            get
            {
                float temp = O;
                temp = A == -1000 || A > temp ? temp : A;
                temp = B == -1000 || B > temp ? temp : B;
                temp = C == -1000 || C > temp ? temp : C;
                temp = D == -1000 || D > temp ? temp : D;
                temp = E == -1000 || E > temp ? temp : E;
                temp = F == -1000 || F > temp ? temp : F;
                temp = G == -1000 || G > temp ? temp : G;
                temp = H == -1000 || H > temp ? temp : H;
                temp = I == -1000 || I > temp ? temp : I;
                temp = J == -1000 || J > temp ? temp : J;
                temp = K == -1000 || K > temp ? temp : K;
                temp = L == -1000 || L > temp ? temp : L;
                temp = M == -1000 || M > temp ? temp : M;
                temp = N == -1000 || N > temp ? temp : N;
                return temp;
            }
        }

        [NotMapped]
        public string StringMaxT { get { return MaxT.ToString("0.00"); } }
        [NotMapped]
        public string StringMinT { get { return MinT.ToString("0.00"); } }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    /// <summary>
    /// 界面显示数据列表
    /// </summary>
    public class TestDataList : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string TemperatureName { get; set; }
        public string TemperatureValue { get; set; }
        public string HumidityValue { get; set; }
        public ObservableCollection<TestData> DataList { get; set; }

        public TestDataList()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            DataList = new ObservableCollection<TestData>();
        }

        #region NotMapped

        [NotMapped]
        public string TemperatureTitle
        {
            get { return TemperatureName + "(" + TemperatureValue + "℃)  "; }
        }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
