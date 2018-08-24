using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace THTS.DataAccess.EntityDAO
{
    public class TestInfoDAO : IDisposable
    {
        SQLiteDB context;

        public TestInfoDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取测试信息
        /// </summary>
        public static TestInfo GetTestInfoData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestInfos.Load();
                if (ctx.TestInfos.Local.Count > 0)
                {
                    return ctx.TestInfos.Local[0];
                }else
                {
                    TestInfo test = new TestInfo();
                    test.Company = "北京XX仪表有限公司";

                    test.UutName = "高低温箱";
                    test.UutModule = "T-001";
                    test.UutSN = "T20180101001";
                    test.UutManufacture = "北京YY有限公司";
                    test.UutCustomSN = "CSN001";
                    test.Accuracy = "1";
                    test.TemperatureLower = "-40";
                    test.TemperatureUpper = "120";

                    test.EnvironmentTemperature = "25";
                    test.EnvironmentPressure = "101";
                    test.EnvironmentHumidity = "45";

                    test.RecordSN = "R20180808001";
                    test.CertificateSN = "SN1234567";

                    test.VerifiedBy = "张三";
                    test.CheckedBy = "李四";
                    test.TestDate = "2018-08-08";

                    test.TempAverageIsChecked = true;
                    test.TempDepartureIsChecked = true;

                    return test;
                }
            }
        }

        /// <summary>
        /// 保存或更新测试信息
        /// </summary>
        /// <returns></returns>
        public static bool SaveOrUpdate(TestInfo testInfo)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                TestInfo oldInfo = GetTestInfoData();

                if (oldInfo.Id == 0)
                {
                    ctx.TestInfos.Add(testInfo);
                }
                else
                {
                    ctx.Update(oldInfo.Id, testInfo);
                }

                return ctx.SaveChanges() > 0;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }

        #endregion
    }
}
