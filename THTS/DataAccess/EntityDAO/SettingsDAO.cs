using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace THTS.DataAccess
{
    public class SettingsDAO : IDisposable
    {
        SQLiteDB context;

        public SettingsDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        public static Settings GetData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.SettingsSet.Where(t => t.Id == 1).Load();
                return ctx.SettingsSet.Local[0];
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
