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

        public string TemperatureDepartureValue { get; set; }
        public string TemperatureAverageValue { get; set; }
        public string TemperatureFluctuationValue { get; set; }
        public string HumidityDepartureValue { get; set; }
        public string HumidityAverageValue { get; set; }
        public string HumidityFluctuationValue { get; set; }

        public TestDataResult()
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

        [NotMapped]
        public ObservableCollection<TestData> DataList { get; set; }

        #endregion

        #region Methods

        public void CalcuteResult()
        {
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
        }

        #endregion


        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
