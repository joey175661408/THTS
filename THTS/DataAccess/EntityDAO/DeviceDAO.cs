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
        /// 获取传感器信息
        /// </summary>
        /// <param name="sensorID"></param>
        /// <returns></returns>
        public static Device GetDevice(string sensorID)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Devices.Where(t => t.IsDelete != 1 && t.SensorId == sensorID).Load();
                if (ctx.Devices.Local.Count > 0)
                {
                    return ctx.Devices.Local[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取最新保存的设备信息
        /// </summary>
        /// <returns></returns>
        public static Device GetLastDevice()
        {
            ObservableCollection<Device> deviceList = GetAllData();
            if (deviceList.Count > 0)
            {
                return deviceList[deviceList.Count - 1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 此传感器是否已存在
        /// </summary>
        /// <param name="sensorID"></param>
        /// <returns></returns>
        public static bool IsExist(string sensorID)
        {
            return GetDevice(sensorID) != null;
        }

        /// <summary>
        /// 此出厂编号是否存在
        /// </summary>
        /// <param name="factoryNO"></param>
        /// <returns></returns>
        public static bool IsExistWithFactoryNo(string factoryNO)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Devices.Where(t => t.IsDelete != 1 && t.FactoryNo == factoryNO).Load();
                if (ctx.Devices.Local.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
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

        /// <summary>
        /// 更新设备信息
        /// </summary>
        /// <param name="newDevice"></param>
        /// <returns></returns>
        public static bool Update(Device newDevice)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                Device old = GetDevice(newDevice.SensorId);

                ctx.Update(old.Id, newDevice);
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="newDevice"></param>
        /// <returns></returns>
        public static bool Delete(Device newDevice)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                Device old = GetDevice(newDevice.SensorId);
                old.IsDelete = 1;
                ctx.Update(old.Id, old);
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
