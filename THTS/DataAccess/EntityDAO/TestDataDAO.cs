using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THTS.DataAccess.Entity;

namespace THTS.DataAccess.EntityDAO
{
    public class TestDataDAO : IDisposable
    {
        SQLiteDB context;

        public TestDataDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        public static ObservableCollection<TestData> GetData(string recordSN)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestDatas.Where(t => t.RecordSN == recordSN).Load();
                return ctx.TestDatas.Local;
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public static bool Save(ObservableCollection<TestData> dataList)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.TestDatas.AddRange(dataList);
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
