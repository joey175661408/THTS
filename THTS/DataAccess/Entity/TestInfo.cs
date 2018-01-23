using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THTS.DataAccess
{
    /// <summary>
    /// 基本测试参数
    /// </summary>
    public class TestInfo
    {
        public string company { get; set; }
        public string uutName { get; set; }
        public string uutModule { get; set; }
        public string uutSN { get; set; }
        public string uutManufacture { get; set; }
        public string uutCustomSN { get; set; }
        public string accuracy { get; set; }
        public string temperatureLower { get; set; }
        public string temperatureUpper { get; set; }
        public string environmentTemperature { get; set; }
        public string environmentPressure { get; set; }
        public string environmentHumidity { get; set; }
        public string recordSN { get; set; }
        public string verifiedBy { get; set; }
        public string checkedBy { get; set; }
        public string testDate { get; set; }
        public string temperatureDeparture { get; set; }
        public string temperatureAverage { get; set; }
        public string temperatureRecovery { get; set; }
        public string temperatureChangeRate { get; set; }
        

    }
}
