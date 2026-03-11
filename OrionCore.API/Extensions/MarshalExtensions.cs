using System;
using System.Runtime.InteropServices;

namespace Orion.Api.Extensions
{
    /// <summary>struct 擴充方法</summary>
    public static class MarshalExtensions
    {

        /// <summary>將結構體轉為位元組陣列。</summary>
        /// <typeparam name="T">結構體型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <returns>對應記憶體內容的位元組陣列。</returns>
        public static byte[] ToByteArray<T>(this T source) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var bytes = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(source, ptr, true);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }


        /// <summary>將位元組陣列還原為指定結構體。</summary>
        /// <typeparam name="T">結構體型別。</typeparam>
        /// <param name="bytes">來源位元組陣列。</param>
        /// <returns>還原後的結構體實例。</returns>
        public static T ToStruct<T>(this byte[] bytes) where T : struct
        {
            Type type = typeof(T);
            int size = Marshal.SizeOf(type);
            if (bytes.Length < size) { throw new ArgumentOutOfRangeException("bytes", size, $"bytes 長度小於 {size}"); }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, ptr, size);
            var result = (T)Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return result;
        }


    }
}
