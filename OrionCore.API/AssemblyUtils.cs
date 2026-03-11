using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Orion.Api.Models;

namespace Orion.Api
{
    /// <summary>提供讀取組件中繼資料的工具方法。</summary>
    public static class AssemblyUtils
    {

        /// <summary>依組件名稱載入組件並取得組件名稱。</summary>
        /// <param name="assemblyString">可供 <see cref="Assembly.Load(string)"/> 使用的組件識別字串。</param>
        /// <returns>組件名稱。</returns>
        public static string GetName(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetName(asm);
        }
        /// <summary>由指定型別取得其所屬組件名稱。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>組件名稱。</returns>
        public static string GetName(Type type)
        {
            return GetName(type.Assembly);
        }
        /// <summary>取得組件名稱。</summary>
        /// <param name="asm">目標組件。</param>
        /// <returns>組件名稱。</returns>
        public static string GetName(Assembly asm)
        {
            return asm.GetName().Name;
        }



        /// <summary>依組件名稱載入組件並取得標題。</summary>
        /// <param name="assemblyString">可供 <see cref="Assembly.Load(string)"/> 使用的組件識別字串。</param>
        /// <returns>組件標題；若未設定則為 <c>null</c>。</returns>
        public static string GetTitle(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetTitle(asm);
        }
        /// <summary>由指定型別取得其所屬組件標題。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>組件標題；若未設定則為 <c>null</c>。</returns>
        public static string GetTitle(Type type)
        {
            return GetTitle(type.Assembly);
        }
        /// <summary>取得組件標題。</summary>
        /// <param name="asm">目標組件。</param>
        /// <returns>組件標題；若未設定則為 <c>null</c>。</returns>
        public static string GetTitle(Assembly asm)
        {
            return asm.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        }


        /// <summary>依組件名稱載入組件並取得版本。</summary>
        /// <param name="assemblyString">可供 <see cref="Assembly.Load(string)"/> 使用的組件識別字串。</param>
        /// <returns>組件版本。</returns>
        public static Version GetVersion(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetVersion(asm);
        }
        /// <summary>由指定型別取得其所屬組件版本。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>組件版本。</returns>
        public static Version GetVersion(Type type)
        {
            return GetVersion(type.Assembly);
        }
        /// <summary>取得組件版本。</summary>
        /// <param name="asm">目標組件。</param>
        /// <returns>組件版本。</returns>
        public static Version GetVersion(Assembly asm)
        {
            return asm.GetName().Version;
        }


        /// <summary>依組件名稱載入組件並取得描述。</summary>
        /// <param name="assemblyString">可供 <see cref="Assembly.Load(string)"/> 使用的組件識別字串。</param>
        /// <returns>組件描述；若未設定則為 <c>null</c>。</returns>
        public static string GetDescription(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetDescription(asm);
        }
        /// <summary>由指定型別取得其所屬組件描述。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>組件描述；若未設定則為 <c>null</c>。</returns>
        public static string GetDescription(Type type)
        {
            return GetDescription(type.Assembly);
        }
        /// <summary>取得組件描述。</summary>
        /// <param name="asm">目標組件。</param>
        /// <returns>組件描述；若未設定則為 <c>null</c>。</returns>
        public static string GetDescription(Assembly asm)
        {
            return asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
        }



        /// <summary>依組件名稱載入組件並建立完整中繼資料。</summary>
        /// <param name="assemblyString">可供 <see cref="Assembly.Load(string)"/> 使用的組件識別字串。</param>
        /// <returns>組件中繼資料物件。</returns>
        public static AssemblyMeta GetMeta(string assemblyString)
        {
            Assembly asm = Assembly.Load(assemblyString);
            return GetMeta(asm);
        }

        /// <summary>由指定型別取得其所屬組件完整中繼資料。</summary>
        /// <param name="type">目標型別。</param>
        /// <returns>組件中繼資料物件。</returns>
        public static AssemblyMeta GetMeta(Type type)
        {
            return GetMeta(type.Assembly);
        }

        /// <summary>建立組件完整中繼資料。</summary>
        /// <param name="asm">目標組件。</param>
        /// <returns>組件中繼資料物件。</returns>
        public static AssemblyMeta GetMeta(Assembly asm)
        {
            var meta = new AssemblyMeta
            {
                Name = GetName(asm),
                Title = GetTitle(asm),
                Version = GetVersion(asm),
                Description = GetDescription(asm),
                Culture = asm.GetName().CultureName,
                Configuration = asm.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration,
                Company = asm.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company,
                Product = asm.GetCustomAttribute<AssemblyProductAttribute>()?.Product,
                Copyright = asm.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright,
                Trademark = asm.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark,
                Guid = asm.GetCustomAttribute<GuidAttribute>()?.Value,
                FileVersion = asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version,
            };

            return meta;
        }


    }
}
