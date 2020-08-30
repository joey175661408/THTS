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
        /// 获取所有查询数据
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<TestInfo> GetAllData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestInfos.Where(t=>t.IsDeleted == 0).Load();
                return ctx.TestInfos.Local;
            }
        }

        /// <summary>
        /// 获取测试信息
        /// </summary>
        public static TestInfo GetTestInfoData(string recordSN)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestInfos.Where(t=> t.IsDeleted == 0 && t.RecordSN == recordSN).Load();
                if (ctx.TestInfos.Local.Count > 0)
                {
                    return ctx.TestInfos.Local[0];
                }
                else
                {
                    return null;
                }
            }
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
                    TestInfo temp = ctx.TestInfos.Local[ctx.TestInfos.Local.Count-1];

                    if(temp.Extra4 == null | temp.Extra5 == null | temp.Extra6 == null)
                    {
                        temp.Extra4 = "False";
                        temp.Extra5 = "False";
                        temp.Extra6 = "False";
                    }

                    return temp;
                }else
                {
                    TestInfo test = new TestInfo();
                    test.Company = "北京XX仪表有限公司";

                    test.UutName = "高低温箱";
                    test.UutModule = "T-001";
                    test.UutSN = "T20180101001";
                    test.UutManufacture = "北京YY有限公司";
                    test.UutCustomSN = "CSN001";
                    test.UutCalPosition = "北京博芮思元仪表科技有限公司";
                    test.Accuracy = "1";
                    test.TemperatureLower = "0";
                    test.TemperatureUpper = "120";
                    test.Extra3 = "2";
                    test.Extra1 = "20";
                    test.Extra2 = "100";
                    test.Extra4 = "False";
                    test.Extra5 = "False";
                    test.Extra6 = "False";

                    test.EnvironmentTemperature = "25";
                    test.EnvironmentPressure = "101";
                    test.EnvironmentHumidity = "45";

                    test.RecordSN = DateTime.Now.ToString("RyyyyMMddHHmmss0");
                    test.CertificateSN = DateTime.Now.ToString("CyyyyMMddHHmmss0");

                    test.VerifiedBy = "张三";
                    test.CheckedBy = "李四";
                    test.TestDate = DateTime.Now.ToString("yyyy-MM-dd");

                    test.TempAverageIsChecked = true;
                    test.TempDepartureIsChecked = true;
                    test.HumiDepartureIsChecked = false;
                    test.HumiAverageIsChecked = false;

                    return test;
                }
            }
        }

        /// <summary>
        /// 此记录编号是否存在
        /// </summary>
        /// <param name="recordSN"></param>
        /// <returns></returns>
        public static bool IsExit(string recordSN)
        {
            return GetTestInfoData(recordSN) != null;
        }

        /// <summary>
        /// 保存或更新测试信息
        /// </summary>
        /// <returns></returns>
        public static bool Save(TestInfo testInfo)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestInfos.Add(testInfo);
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="testInfo"></param>
        /// <returns></returns>
        public static bool Delete(TestInfo testInfo)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                TestInfo temp = GetTestInfoData(testInfo.RecordSN);
                if(temp != null)
                {
                    temp.IsDeleted = 1;
                }
                ctx.Update(temp.Id, temp);
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

    public class PositionAndSensorDAO : IDisposable
    {
        SQLiteDB context;

        public PositionAndSensorDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public static ObservableCollection<PositionAndSensor> GetPositionAndSensor(string recordSN)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.PositionAndSensors.Where(t => t.IsDeleted == 0 && t.RecordSN == recordSN).Load();
                if (ctx.PositionAndSensors.Local.Count > 0)
                {
                    return ctx.PositionAndSensors.Local;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public static bool Save(ObservableCollection<PositionAndSensor> info)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.PositionAndSensors.AddRange(info);
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="testInfo"></param>
        /// <returns></returns>
        public static bool Delete(string recordSN)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ObservableCollection<PositionAndSensor> temp = GetPositionAndSensor(recordSN);
                if (temp != null)
                {
                    foreach (var item in temp)
                    {
                        item.IsDeleted = 1;
                        ctx.Update(item.Id, item);
                    }
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
