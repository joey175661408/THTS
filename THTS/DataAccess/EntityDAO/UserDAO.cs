using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace THTS.DataAccess
{
    public class UserDAO : IDisposable
    {
        SQLiteDB context;

        public UserDAO()
        {
            context = new SQLiteDB();
        }

        /// <summary>
        /// 获取所有设备信息
        /// </summary>
        public static ObservableCollection<User> GetAllData()
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Users.Where(t => t.IsDelete != 1).Load();
                return ctx.Users.Local;
            }
        }


        /// <summary>
        /// 根据输入的用户名判断该用户名是否已经存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool IsLogin(string userName, string password)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Users.Where(t => t.IsDelete != 1).Where(t => t.UserName.Equals(userName)).Where(t => t.Password.Equals(password)).Load();
                return ctx.Users.Local.Count > 0;
            }
        }

        /// <summary>
        /// 根据输入的用户名判断该用户名是否已经存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool IsExist(string userName)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Users.Where(t => t.IsDelete != 1).Where(t => t.UserName.Equals(userName)).Load();
                return ctx.Users.Local.Count > 0;
            }
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public static bool SaveOrUpdate(User newUser)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                if (IsExist(newUser.UserName))
                {
                    ctx.Update(newUser.Id, newUser);
                }else
                {
                    ctx.Users.Add(newUser);
                }
                return ctx.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="selectedUser"></param>
        /// <returns></returns>
        public static bool Delete(User selectedUser)
        {
            selectedUser.IsDelete = 1;
            return SaveOrUpdate(selectedUser);
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
