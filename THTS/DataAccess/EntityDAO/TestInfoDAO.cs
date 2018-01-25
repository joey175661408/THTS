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
                    return new TestInfo();
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
