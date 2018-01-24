using System;
using System.Linq;
using System.Data.Entity;

namespace THTS.DataAccess
{
    public class UserDAO : IDisposable
    {
        SQLiteDB context;

        public UserDAO()
        {
            context = new SQLiteDB();
        }

        public static bool test()
        {
            using (var db = new SQLiteDB())
            {
                User fv = new User()
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin",
                    IsDelete = 0
                };
                db.Users.Add(fv);
                db.SaveChanges();
            }

            return true;
        }

        /// <summary>
        /// 根据输入的用户名判断该用户名是否已经存在
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static bool IsExist(string userName, string password)
        {
            using (SQLiteDB ctx = new SQLiteDB())
            {
                ctx.Users.Where(t => t.IsDelete != 1).Where(t => t.UserName.Equals(userName)).Where(t=> t.Password.Equals(password)).Load();
                return ctx.Users.Local.Count > 0;
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
