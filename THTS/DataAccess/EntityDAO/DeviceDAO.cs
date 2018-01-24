using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace THTS.DataAccess.EntityDAO
{
    public class DeviceDAO :IDisposable
    {
        SQLiteDB context;

        public DeviceDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        public static ObservableCollection<Device> GetAllData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Devices.Where(t => t.IsDelete != 1).Load();
                return ctx.Devices.Local;
            }
        }

        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <param name="newDevice"></param>
        /// <returns></returns>
        public static bool Save(Device newDevice)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Devices.Add(newDevice);
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
