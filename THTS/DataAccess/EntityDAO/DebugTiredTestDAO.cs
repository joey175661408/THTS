using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Collections.Generic;


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
        /// 获取所有任务名称
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTaskName()
        {
            using(SQLiteDB ctx = new SQLiteDB())
            {
                return ctx.DebugTiredTests.AsEnumerable().Select(t => t.TaskName).Distinct().ToList();
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
