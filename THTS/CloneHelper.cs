using System;
using System.Reflection;

namespace THTS
{
    public class CloneHelper
    {
        /// <summary>
        /// 克隆一个对象
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Object CloneObject(Object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }
    }
}
