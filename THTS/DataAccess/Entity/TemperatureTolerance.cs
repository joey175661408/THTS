using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THTS.DataAccess
{
    /// <summary>
    /// 温度偏差、波动度及均匀度测试参数
    /// </summary>
    public class TemperatureTolerance
    {
        public int Id { get; set;}

        #region 测试温度参数
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
        #endregion

        public string Atmosphere { get; set; }
        public string WindSpeed { get; set; }

        public string Channel { get; set; }
        public string SensorIndex { get; set; }
        public string SensorName { get; set; }
        public string SensorSN { get; set; }

        public string IsCJC { get; set; }
        public string ChannelCJC { get; set; }
        public int TypeCJC { get; set; }
        public string TempertureCJC { get; set; }

        public string TestTimeSpan { get; set; }
        public string TestSampleInterval { get; set; }

        public int ContainMedium { get; set; }
        public string MediumDescription { get; set; }
        public int ContainLoad { get; set; }
        public string LoadDescription { get; set; }

    }
}
