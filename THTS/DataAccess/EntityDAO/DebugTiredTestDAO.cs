using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;


namespace THTS.DataAccess
{
    public class DebugTiredTestDAO : IDisposable
    {
        SQLiteDB context;

        public DebugTiredTestDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取采集的测试数据
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static ObservableCollection<DebugTiredTest> GetAllData(string taskName)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.DebugTiredTests.Where(t => t.TaskName == taskName).Load();
                return ctx.DebugTiredTests.Local;
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public static bool Save(ObservableCollection<DebugTiredTest> data)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.DebugTiredTests.AddRange(data);
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
