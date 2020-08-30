using System.ComponentModel;
using THTS.MVVM;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Navigation;

namespace THTS.DataAccess
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public class Device : INotifyPropertyChanged, ISequencedObject
    {
        public int Id { get; set; }
        public string SensorId { get; set; }
        public string ModuleName { get; set; }
        public string Manufacture { get; set; }
        public string FactoryNo { get; set; }
        public string CertificateNo { get; set; }
        public int CalibrateResult { get; set; }
        public string CalibrateDate { get; set; }
        public string ExpireDate { get; set; }
        public string Remark { get; set; }
        public int IsDelete { get; set; }

        /// <summary>
        /// 标准值1
        /// </summary>
        public string ReferenceValue1 { get; set; }
        /// <summary>
        /// 实际示值1
        /// </summary>
        public string RealValue1 { get; set; }
        /// <summary>
        /// 修正值1
        /// </summary>
        public string ToleranceValue1 { get; set; }
        /// <summary>
        /// 标准值2
        /// </summary>
        public string ReferenceValue2 { get; set; }
        /// <summary>
        /// 实际示值2
        /// </summary>
        public string RealValue2 { get; set; }
        /// <summary>
        /// 修正值2
        /// </summary>
        public string ToleranceValue2 { get; set; }
        /// <summary>
        /// 标准值3
        /// </summary>
        public string ReferenceValue3 { get; set; }
        /// <summary>
        /// 实际示值3
        /// </summary>
        public string RealValue3 { get; set; }
        /// <summary>
        /// 修正值3
        /// </summary>
        public string ToleranceValue3 { get; set; }
        /// <summary>
        /// 是否修正
        /// </summary>
        public string IsCalculate { get; set; }
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

        [NotMapped]
        public string CalibrateResultString
        {
            get
            {
                return CalibrateResult == 1 ? "合格" : "不合格";
            }
        }

        public void Calcute()
        {
            float Rvalue1, Rvalue2, Rvalue3, Revalue1, Revalue2, Revalue3;
            if(float.TryParse(ReferenceValue1, out Rvalue1)&& float.TryParse(RealValue1, out Revalue1))
            {
                ToleranceValue1 = (Rvalue1 - Revalue1).ToString("F2");
            }

            if (float.TryParse(ReferenceValue2, out Rvalue2) && float.TryParse(RealValue2, out Revalue2))
            {
                ToleranceValue2 = (Rvalue2 - Revalue2).ToString("F2");
            }

            if (float.TryParse(ReferenceValue3, out Rvalue3) && float.TryParse(RealValue3, out Revalue3))
            {
                ToleranceValue3 = (Rvalue3 - Revalue3).ToString("F2");
            }
        }

        /// <summary>
        /// 基于修正值计算最终示值
        /// </summary>
        public float CalcWithTolerance(float value,string modulename)
        {
            float R1, R2, R3, E1, E2, E3;

            //修正后的结果
            float valueWithTolerance = -1000;

            if (modulename.Contains("PT100"))
            {
                #region PT100温度传感器 y=ax+b
            
                if (float.TryParse(ReferenceValue1, out R1) && float.TryParse(RealValue1, out E1)
                    && float.TryParse(ReferenceValue2, out R2) && float.TryParse(RealValue2, out E2))
                {
                    float a = (E2 - E1) / (R2 - R1);
                    float b = E1;
                    valueWithTolerance = a * value + b;
                }

                #endregion
            }
            else
            {
                #region 湿度传感器/K型热电偶温度传感器 y=ax2+bx+c

                if (float.TryParse(ReferenceValue1, out R1) && float.TryParse(RealValue1, out E1)
                    && float.TryParse(ReferenceValue2, out R2) && float.TryParse(RealValue2, out E2)
                    && float.TryParse(ReferenceValue3, out R3) && float.TryParse(RealValue3, out E3))
                {
                    float a = ((E3 - E1) / (R3 - R1) - (E2 - E1) / (R2 - R1)) / (R3 - R2);
                    float b = (E3 - E1) / (R3 - R1) - a * (R3 + R1);
                    float c = E1 - a * R1 * R1 - b * R1;
                    valueWithTolerance = a * value * value + b * value + c;
                }
                #endregion
            }


            return valueWithTolerance;
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
