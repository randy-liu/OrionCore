using System;
using System.Runtime.InteropServices;

namespace Orion.Api.Extensions
{
    /// <summary>struct 擴充方法</summary>
    public static class MarshalExtensions
    {

        /// <summary>取得 struct 的 byte array</summary>
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


        /// <summary>取得 struct 的 byte array</summary>
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
