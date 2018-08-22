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
        public static Setting GetData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Settingses.Where(t => t.No.Equals("1")).Load();

                if (ctx.Settingses.Local.Count < 1)
                {
                    Setting temp = new Setting() { No = "1", PortName = "COM1", BaudRate = 115200 };
                    SaveOrUpdate(temp);
                    return temp;
                }

                return ctx.Settingses.Local[0];
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public static bool SaveOrUpdate(Setting settings)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Settingses.Load();
                if (ctx.Settingses.Local.Count > 0)
                {
                    ctx.Update(settings.No, settings);
                }
                else
                {
                    ctx.Settingses.Add(settings);
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
