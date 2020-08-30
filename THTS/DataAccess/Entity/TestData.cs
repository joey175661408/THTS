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
        /// 5点1/9点1/15点1/27点1
        /// </summary>
        public float A { get; set; }
        /// <summary>
        /// 5点2/9点2/15点2/27点2
        /// </summary>
        public float B { get; set; }
        /// <summary>
        /// 5点3/9点3/15点3/27点3
        /// </summary>
        public float C { get; set; }
        /// <summary>
        /// 5点4/9点4/15点4/27点4
        /// </summary>
        public float D { get; set; }
        /// <summary>
        /// 9点6/15点6/27点6
        /// </summary>
        public float E { get; set; }
        /// <summary>
        /// 9点7/15点7/27点7
        /// </summary>
        public float F { get; set; }
        /// <summary>
        /// 9点8/15点8/27点8
        /// </summary>
        public float G { get; set; }
        /// <summary>
        /// 9点9/15点9/27点9
        /// </summary>
        public float H { get; set; }
        /// <summary>
        /// 15点10/27点10
        /// </summary>
        public float I { get; set; }
        /// <summary>
        /// 15点11/27点11
        /// </summary>
        public float J { get; set; }
        /// <summary>
        /// 15点12/27点12
        /// </summary>
        public float K { get; set; }
        /// <summary>
        /// 15点13/27点13
        /// </summary>
        public float L { get; set; }
        /// <summary>
        /// 15点14/27点14
        /// </summary>
        public float M { get; set; }
        /// <summary>
        /// 15点15/27点15
        /// </summary>
        public float N { get; set; }
        /// <summary>
        /// 5点5/9点5/15点5/27点5
        /// </summary>
        public float O { get; set; }

        /// <summary>
        /// 27点16
        /// </summary>
        public float T16 { get; set; }
        /// <summary>
        /// 27点17
        /// </summary>
        public float T17 { get; set; }
        /// <summary>
        /// 27点18
        /// </summary>
        public float T18 { get; set; }
        /// <summary>
        /// 27点19
        /// </summary>
        public float T19 { get; set; }
        /// <summary>
        /// 27点20
        /// </summary>
        public float T20 { get; set; }
        /// <summary>
        /// 27点21
        /// </summary>
        public float T21 { get; set; }
        /// <summary>
        /// 27点22
        /// </summary>
        public float T22 { get; set; }
        /// <summary>
        /// 27点23
        /// </summary>
        public float T23 { get; set; }
        /// <summary>
        /// 27点24
        /// </summary>
        public float T24 { get; set; }
        /// <summary>
        /// 27点25
        /// </summary>
        public float T25 { get; set; }
        /// <summary>
        /// 27点26
        /// </summary>
        public float T26 { get; set; }
        /// <summary>
        /// 27点27
        /// </summary>
        public float T27 { get; set; }

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
        public string StringCount { get { return Count.Equals(0) ? "" : Count.ToString(); } }
        [NotMapped]
        public string StringO { get { return O.Equals(-1000) ? "" : O.ToString("0.00"); } }
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
        public string StringT16 { get { return T16.Equals(-1000) ? "" : T16.ToString("0.00"); } }
        [NotMapped]
        public string StringT17 { get { return T17.Equals(-1000) ? "" : T17.ToString("0.00"); } }
        [NotMapped]
        public string StringT18 { get { return T18.Equals(-1000) ? "" : T18.ToString("0.00"); } }
        [NotMapped]
        public string StringT19 { get { return T19.Equals(-1000) ? "" : T19.ToString("0.00"); } }
        [NotMapped]
        public string StringT20 { get { return T20.Equals(-1000) ? "" : T20.ToString("0.00"); } }
        [NotMapped]
        public string StringT21 { get { return T21.Equals(-1000) ? "" : T21.ToString("0.00"); } }
        [NotMapped]
        public string StringT22 { get { return T22.Equals(-1000) ? "" : T22.ToString("0.00"); } }
        [NotMapped]
        public string StringT23 { get { return T23.Equals(-1000) ? "" : T23.ToString("0.00"); } }
        [NotMapped]
        public string StringT24 { get { return T24.Equals(-1000) ? "" : T24.ToString("0.00"); } }
        [NotMapped]
        public string StringT25 { get { return T25.Equals(-1000) ? "" : T25.ToString("0.00"); } }
        [NotMapped]
        public string StringT26 { get { return T26.Equals(-1000) ? "" : T26.ToString("0.00"); } }
        [NotMapped]
        public string StringT27 { get { return T27.Equals(-1000) ? "" : T27.ToString("0.00"); } }
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
                float temp = -1000;
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
                temp = T16 == -1000 || T16 < temp ? temp : T16;
                temp = T17 == -1000 || T17 < temp ? temp : T17;
                temp = T18 == -1000 || T18 < temp ? temp : T18;
                temp = T19 == -1000 || T19 < temp ? temp : T19;
                temp = T20 == -1000 || T20 < temp ? temp : T20;
                temp = T21 == -1000 || T21 < temp ? temp : T21;
                temp = T22 == -1000 || T22 < temp ? temp : T22;
                temp = T23 == -1000 || T23 < temp ? temp : T23;
                temp = T24 == -1000 || T24 < temp ? temp : T24;
                temp = T25 == -1000 || T25 < temp ? temp : T25;
                temp = T26 == -1000 || T26 < temp ? temp : T26;
                temp = T27 == -1000 || T27 < temp ? temp : T27;
                return temp;
            }
        }

        [NotMapped]
        public float MinT
        {
            get
            {
                float temp = 1000;
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
                temp = T16 == -1000 || T16 > temp ? temp : T16;
                temp = T17 == -1000 || T17 > temp ? temp : T17;
                temp = T18 == -1000 || T18 > temp ? temp : T18;
                temp = T19 == -1000 || T19 > temp ? temp : T19;
                temp = T20 == -1000 || T20 > temp ? temp : T20;
                temp = T21 == -1000 || T21 > temp ? temp : T21;
                temp = T22 == -1000 || T22 > temp ? temp : T22;
                temp = T23 == -1000 || T23 > temp ? temp : T23;
                temp = T24 == -1000 || T24 > temp ? temp : T24;
                temp = T25 == -1000 || T25 > temp ? temp : T25;
                temp = T26 == -1000 || T26 > temp ? temp : T26;
                temp = T27 == -1000 || T27 > temp ? temp : T27;
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
                float temp = -1000;
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
                float temp = 1000;
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
