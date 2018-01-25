using System;
using System.Data.Entity;

namespace THTS.DataAccess.EntityDAO
{
    public class TemperatureToleranceDAO : IDisposable
    {
        SQLiteDB context;

        public TemperatureToleranceDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取测试信息
        /// </summary>
        public static TemperatureTolerance GetToleranceInfoData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TemperatureTolerances.Load();
                if (ctx.TemperatureTolerances.Local.Count > 0)
                {
                    return ctx.TemperatureTolerances.Local[0];
                }else
                {
                    return new TemperatureTolerance();
                }
            }
        }

        /// <summary>
        /// 保存或更新测试信息
        /// </summary>
        /// <returns></returns>
        public static bool SaveOrUpdate(TemperatureTolerance toleranceInfo)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                TemperatureTolerance oldInfo = GetToleranceInfoData();

                if (oldInfo.Id == 0)
                {
                    ctx.TemperatureTolerances.Add(toleranceInfo);
                }
                else
                {
                    ctx.Update(oldInfo.Id, toleranceInfo);
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
