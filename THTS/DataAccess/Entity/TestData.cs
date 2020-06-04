using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows;
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
        /// <summary>
        /// 5点1/9点1/15点1
        /// </summary>
        public float A { get; set; }
        /// <summary>
        /// 5点2/9点2/15点2
        /// </summary>
        public float B { get; set; }
        /// <summary>
        /// 5点3/9点3/15点3
        /// </summary>
        public float C { get; set; }
        /// <summary>
        /// 5点4/9点4/15点4
        /// </summary>
        public float D { get; set; }
        /// <summary>
        /// 9点6/15点6
        /// </summary>
        public float E { get; set; }
        /// <summary>
        /// 9点7/15点7
        /// </summary>
        public float F { get; set; }
        /// <summary>
        /// 9点8/15点8
        /// </summary>
        public float G { get; set; }
        /// <summary>
        /// 9点9/15点9
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// 15点10
        /// </summary>
        public float I { get; set; }
        /// <summary>
        /// 15点11
        /// </summary>
        public float J { get; set; }
        /// <summary>
        /// 15点12
        /// </summary>
        public float K { get; set; }
        /// <summary>
        /// 15点13
        /// </summary>
        public float L { get; set; }
        /// <summary>
        /// 15点14
        /// </summary>
        public float M { get; set; }
        /// <summary>
        /// 15点15
        /// </summary>
        public float N { get; set; }
        /// <summary>
        /// 5点5/9点5/15点5
        /// </summary>
        public float O { get; set; }

        /// <summary>
        /// 5点O/9点O/15点O
        /// </summary>
        public float Jia { get; set; }
        /// <summary>
        /// 5点A/9点A/15点A
        /// </summary>
        public float Yi { get; set; }
        /// <summary>
        /// 5点B/9点B/15点B
        /// </summary>
        public float Bing{ get; set; }
        /// <summary>
        /// 9点C/15点C
        /// </summary>
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
        [NotMapped]
        public string StringAverageT { get; set; }


        [NotMapped]
        public float MaxH
        {
            get
            {
                float temp = Jia;
                temp = Yi == -1000 || Yi < temp ? temp : Yi;
                temp = Bing == -1000 || Bing < temp ? temp : Bing;
                temp = Ding == -1000 || Ding < temp ? temp : Ding;
                return temp;
            }
        }

        [NotMapped]
        public float MinH
        {
            get
            {
                float temp = Jia;
                temp = Yi == -1000 || Yi > temp ? temp : Yi;
                temp = Bing == -1000 || Bing > temp ? temp : Bing;
                temp = Ding == -1000 || Ding > temp ? temp : Ding;

                return temp;
            }
        }

        [NotMapped]
        public string StringMaxH { get { return MaxH.ToString("0.00"); } }
        [NotMapped]
        public string StringMinH { get { return MinH.ToString("0.00"); } }
        public string StringAverageH { get; set; }
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
            get
            {
                if (string.IsNullOrEmpty(HumidityValue))
                {
                    return TemperatureName + "(" + TemperatureValue + "℃)  ";

                }
                else
                {
                    return TemperatureName + "(" + TemperatureValue + "℃|" + HumidityValue + "%)  ";

                }
            }
        }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
