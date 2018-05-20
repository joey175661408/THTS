using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace THTS.DataModule
{
    public enum EnumUnit
    {
        [Description("")]
        Unkonwn = -1,
        [Description("K")]
        Kelvin = 1000,
        [Description("℃")]
        Celsius = 1001,
        [Description("℉")]
        Fahrenheit = 1002,
        [Description("%")]
        Percent = 1342
    }

    public abstract class EnumShell
    {
        static Dictionary<string, EnumShell> pool = new Dictionary<string, EnumShell>();

        public static EnumShell<T> GetShell<T>(T Instance)
        {
            var key = typeof(T).ToString() + Instance.ToString();
            if (pool.ContainsKey(key))
            {
                return (EnumShell<T>)pool[key];
            }
            else
            {
                var nsh = new EnumShell<T>(Instance);
                pool.Add(key, nsh);
                return nsh;
            }
        }
    }

    public class EnumShell<T> : EnumShell
    {
        internal EnumShell(T Instance)
        {
            this.Instance = Instance;
        }

        public T Instance { get; set; }

        public Type EnumType { get { return typeof(T); } }

        public string Description
        {
            get
            {
                string strValue = Name;
                FieldInfo fieldinfo = Instance.GetType().GetField(strValue);
                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
        }

        public string Name { get { return Instance.ToString(); } }
        public string FullName { get { return EnumType.ToString() + Name; } }

        public T[] BrotherInstances { get { return (T[])Enum.GetValues(this.EnumType); } }

        public EnumShell<T>[] Brothers
        {
            get
            {
                return BrotherInstances.Select(i => EnumShell.GetShell(i)).ToArray();
            }
        }
    }
}
