using System;
using System.Runtime.InteropServices;

namespace THTS.SerialPort
{
    unsafe public class Win32
    {
        /// <summary>
        /// 内存块拷贝
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(void* dest, void* source, int size);
    }
}
