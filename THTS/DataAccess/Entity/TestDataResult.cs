using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.MVVM;

namespace THTS.DataAccess.Entity
{
    public class TestDataResult : INotifyPropertyChanged
    {
        public string RecordSN { get; set; }
        public string TemperatureName { get; set; }
        public string TemperatureValue { get; set; }
        public string HumidityValue { get; set; }

        public string TemperatureDepartureUpValue { get; set; }
        public string TemperatureDepartureDownValue { get; set; }
        public string TemperatureAverageValue { get; set; }
        public string TemperatureFluctuationValue { get; set; }
        public string HumidityDepartureUpValue { get; set; }
        public string HumidityDepartureDownValue { get; set; }
        public string HumidityAverageValue { get; set; }
        public string HumidityFluctuationValue { get; set; }
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
        public TestDataResult()
        {
            this.RegisterPropertyChangedHandler(() => PropertyChanged);
            DataList = new ObservableCollection<TestData>();
            FluctuationList = new Dictionary<string, TestDataFluctuationValue>();
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

        [NotMapped]
        public ObservableCollection<TestData> DataList { get; set; }

        /// <summary>
        /// 各个测试点的波动度
        /// </summary>
        [NotMapped]
        public Dictionary<string,TestDataFluctuationValue> FluctuationList { get; set; }
        #endregion

        #region Methods

        public void CalcuteResult()
        {
            //根据测试点分组，计算单个测试点波动度
            GroupByPosition();

            #region 2017规程计算方法

            #region 温度数据
            double sumMMT = 0D;
            double maxT = Double.Parse(DataList[0].StringO);
            double minT = Double.Parse(DataList[0].StringO);

            for (int i = 0; i < DataList.Count; i++)
            {
                sumMMT += Double.Parse(DataList[i].StringMaxT) - Double.Parse(DataList[i].StringMinT);
                DataList[i].StringAverageT = (sumMMT / (i + 1)).ToString("0.00");

                maxT = DataList[i].MaxT > maxT ? DataList[i].MaxT : maxT;
                minT = DataList[i].MinT < minT ? DataList[i].MinT : minT;
            }

            //温度上偏差
            this.TemperatureDepartureUpValue = (maxT - Double.Parse(DataList[0].TemperatureValue)).ToString("0.00");
            //温度下偏差
            this.TemperatureDepartureDownValue = (minT - Double.Parse(DataList[0].TemperatureValue)).ToString("0.00");
            //温度均匀度
            this.TemperatureAverageValue = (sumMMT / DataList.Count).ToString("0.00");
            //温度波动度
            double maxF = 0D;
            for (int i = 1; i <= 15; i++)
            {
                TestDataFluctuationValue Ftemp = FluctuationList[i.ToString()];

                double tempF = 0D;
                if(Double.TryParse(Ftemp.FluctuationValue.Replace("±",""), out tempF))
                {
                    maxF = maxF > tempF ? maxF : tempF;
                }
            }
            this.TemperatureFluctuationValue = "±" + maxF.ToString("0.00");
            #endregion

            #region 湿度数据
            if (!string.IsNullOrEmpty(HumidityValue))
            {
                double sumMMH = 0D;
                double maxH = Double.Parse(DataList[0].StringJia);
                double minH = Double.Parse(DataList[0].StringJia);

                for (int i = 0; i < DataList.Count; i++)
                {
                    sumMMH += Double.Parse(DataList[i].StringMaxH) - Double.Parse(DataList[i].StringMinH);
                    DataList[i].StringAverageH = (sumMMH / (i + 1)).ToString("0.00");

                    maxH = DataList[i].MaxH > maxH ? DataList[i].MaxH : maxH;
                    minH = DataList[i].MinH < minH ? DataList[i].MinH : minH;
                }

                //湿度上偏差
                this.HumidityDepartureUpValue = (maxH - Double.Parse(DataList[0].HumidityValue)).ToString("0.00");
                //湿度下偏差
                this.HumidityDepartureDownValue = (minH - Double.Parse(DataList[0].HumidityValue)).ToString("0.00");
                //湿度均匀度
                this.HumidityAverageValue = (sumMMH / DataList.Count).ToString("0.00");
                
                //湿度波动度
                double maxFH = 0D;
                TestDataFluctuationValue FtempO = FluctuationList["O"];
                maxFH = GetMaxFH(maxFH, FtempO.FluctuationValue.Replace("±", ""));
                TestDataFluctuationValue FtempA = FluctuationList["A"];
                maxFH = GetMaxFH(maxFH, FtempA.FluctuationValue.Replace("±", ""));
                TestDataFluctuationValue FtempB = FluctuationList["B"];
                maxFH = GetMaxFH(maxFH, FtempB.FluctuationValue.Replace("±", ""));
                TestDataFluctuationValue FtempC = FluctuationList["C"];
                maxFH = GetMaxFH(maxFH, FtempC.FluctuationValue.Replace("±", ""));

                this.HumidityFluctuationValue = "±" + maxFH.ToString("0.00");
            }
            #endregion

            #endregion

            #region 2003规程计算方法
            /*
            //温度数据
            double sumOT = 0D;
            double sumDeviceT = 0D;
            double sumMMT = 0D;
            double maxOT = Double.Parse(DataList[0].StringO);
            double minOT = Double.Parse(DataList[0].StringO);

            for (int i = 0; i < DataList.Count; i++)
            {
                sumOT += Double.Parse(DataList[i].StringO);
                sumDeviceT += Double.Parse(DataList[i].DeviceTemperature);
                sumMMT += Double.Parse(DataList[i].StringMaxT) - Double.Parse(DataList[i].StringMinT);
                maxOT = DataList[i].O > maxOT ? Double.Parse(DataList[i].StringO) : maxOT;
                minOT = DataList[i].O < minOT ? Double.Parse(DataList[i].StringO) : minOT;
            }

            this.TemperatureDepartureValue = ((sumDeviceT - sumOT) / DataList.Count).ToString("0.00");
            this.TemperatureAverageValue = (sumMMT / DataList.Count).ToString("0.00");
            this.TemperatureFluctuationValue = "±" + ((maxOT - minOT) / 2).ToString("0.00");

            //湿度数据
            if (!string.IsNullOrEmpty(HumidityValue))
            {
                double sumJiaH = 0D;
                double sumDeviceH = 0D;
                double sumMMH = 0D;
                double maxJiaH = Double.Parse(DataList[0].StringJia);
                double minJiaH = Double.Parse(DataList[0].StringJia);

                for (int i = 0; i < DataList.Count; i++)
                {
                    sumJiaH += Double.Parse(DataList[i].StringJia);
                    sumDeviceH += Double.Parse(DataList[i].DeviceHumidity);
                    sumMMH += Double.Parse(DataList[i].StringMaxH) - Double.Parse(DataList[i].StringMinH);
                    maxJiaH = DataList[i].Jia > maxJiaH ? Double.Parse(DataList[i].StringJia) : maxJiaH;
                    minJiaH = DataList[i].Jia < minJiaH ? Double.Parse(DataList[i].StringJia) : minJiaH;
                }

                this.HumidityDepartureValue = ((sumDeviceH - sumJiaH) / DataList.Count).ToString("0.00");
                this.HumidityAverageValue = (sumMMH / DataList.Count).ToString("0.00");
                this.HumidityFluctuationValue = "±" + ((maxJiaH - minJiaH) / 2).ToString("0.00");
            }
            */
            #endregion

        }

        private static double GetMaxFH(double maxFH, string fluctValue)
        {
            double tempF = 0D;
            if (Double.TryParse(fluctValue, out tempF))
            {
                maxFH = maxFH > tempF ? maxFH : tempF;
            }

            return maxFH;
        }

        private void GroupByPosition()
        {
            TestDataFluctuationValue F1 = new TestDataFluctuationValue();
            TestDataFluctuationValue F2 = new TestDataFluctuationValue();
            TestDataFluctuationValue F3 = new TestDataFluctuationValue();
            TestDataFluctuationValue F4 = new TestDataFluctuationValue();
            TestDataFluctuationValue F5 = new TestDataFluctuationValue();
            TestDataFluctuationValue F6 = new TestDataFluctuationValue();
            TestDataFluctuationValue F7 = new TestDataFluctuationValue();
            TestDataFluctuationValue F8 = new TestDataFluctuationValue();
            TestDataFluctuationValue F9 = new TestDataFluctuationValue();
            TestDataFluctuationValue F10 = new TestDataFluctuationValue();
            TestDataFluctuationValue F11 = new TestDataFluctuationValue();
            TestDataFluctuationValue F12 = new TestDataFluctuationValue();
            TestDataFluctuationValue F13 = new TestDataFluctuationValue();
            TestDataFluctuationValue F14 = new TestDataFluctuationValue();
            TestDataFluctuationValue F15 = new TestDataFluctuationValue();
            TestDataFluctuationValue FA = new TestDataFluctuationValue();
            TestDataFluctuationValue FB = new TestDataFluctuationValue();
            TestDataFluctuationValue FC = new TestDataFluctuationValue();
            TestDataFluctuationValue FO = new TestDataFluctuationValue();

            for (int i = 0; i < DataList.Count; i++)
            {
                F1.DataValue.Add(DataList[i].StringA);
                F2.DataValue.Add(DataList[i].StringB);
                F3.DataValue.Add(DataList[i].StringC);
                F4.DataValue.Add(DataList[i].StringD);
                F5.DataValue.Add(DataList[i].StringO);
                F6.DataValue.Add(DataList[i].StringE);
                F7.DataValue.Add(DataList[i].StringF);
                F8.DataValue.Add(DataList[i].StringG);
                F9.DataValue.Add(DataList[i].StringH);
                F10.DataValue.Add(DataList[i].StringI);
                F11.DataValue.Add(DataList[i].StringJ);
                F12.DataValue.Add(DataList[i].StringK);
                F13.DataValue.Add(DataList[i].StringL);
                F14.DataValue.Add(DataList[i].StringM);
                F15.DataValue.Add(DataList[i].StringN);
                FA.DataValue.Add(DataList[i].StringYi);
                FB.DataValue.Add(DataList[i].StringBing);
                FC.DataValue.Add(DataList[i].StringDing);
                FO.DataValue.Add(DataList[i].StringJia);
            }

            FluctuationList.Add("1", F1);
            FluctuationList.Add("2", F2);
            FluctuationList.Add("3", F3);
            FluctuationList.Add("4", F4);
            FluctuationList.Add("5", F5);
            FluctuationList.Add("6", F6);
            FluctuationList.Add("7", F7);
            FluctuationList.Add("8", F8);
            FluctuationList.Add("9", F9);
            FluctuationList.Add("10", F10);
            FluctuationList.Add("11", F11);
            FluctuationList.Add("12", F12);
            FluctuationList.Add("13", F13);
            FluctuationList.Add("14", F14);
            FluctuationList.Add("15", F15);
            FluctuationList.Add("A", FA);
            FluctuationList.Add("B", FB);
            FluctuationList.Add("C", FC);
            FluctuationList.Add("O", FO);

            foreach (var item in FluctuationList.Keys)
            {
                TestDataFluctuationValue Ftemp = FluctuationList[item];
                double maxValue = 0D;
                double minValue = 0D;
                double sumValue = 0D;
                bool isData = false;
                for (int i = 0; i < Ftemp.DataValue.Count; i++)
                {
                    double tempValue = double.NaN;
                    if (double.TryParse(Ftemp.DataValue[i], out tempValue)){
                        if(i == 0)
                        {
                            maxValue = tempValue;
                            minValue = tempValue;
                            sumValue += tempValue;
                        }
                        else
                        {
                            maxValue = maxValue > tempValue ? maxValue : tempValue;
                            minValue = minValue < tempValue ? minValue : tempValue;
                            sumValue += tempValue;
                        }
                        isData = true;
                    }
                }
                FluctuationList[item].MaxValue = isData ? maxValue.ToString("0.00") : "";
                FluctuationList[item].MinValue = isData ? minValue.ToString("0.00") : "";
                FluctuationList[item].AverageValue = isData ? (sumValue / Ftemp.DataValue.Count).ToString("0.00") : "";
                FluctuationList[item].FluctuationValue = isData ? "±" + ((maxValue - minValue) / 2).ToString("0.00") : "";
            }

        }
        #endregion


        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class TestDataFluctuationValue
    {
        public string Id { get; set; }
        public ObservableCollection<string> DataValue { get; set; }
        public string MaxValue { get; set; }
        public string MinValue { get; set; }
        public string AverageValue { get; set; }
        public string FluctuationValue { get; set; }
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

        public TestDataFluctuationValue()
        {
            DataValue = new ObservableCollection<string>();
        }
    }
}
