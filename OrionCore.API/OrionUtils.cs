using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using Orion.Api.Extensions;

namespace Orion.Api
{
    /// <summary>Utility Tools</summary>
    public static class OrionUtils
    {

		/// <summary>
		/// 有效日期下限
		/// 和 SQL Server DateTime(this DataRow, string) 可接受的最小日期（1753-01-01）對齊。
		/// </summary>
		public static readonly DateTime ValidDate = new DateTime(1753, 1, 1);



        /// <summary>判斷 value( Enum, string, DateTime, DateTimeOffset, IEnumerable, decimal ) 是否有值 </summary>
        /// <param name="value">要檢查是否為有效內容的物件值。</param>
        public static bool HasValue(object value)
        {
            if (value == null) { return false; }
            if (value is DBNull) { return false; }

            if (value is bool) { return true; }
            if (value is Enum) { return true; }


            if (value is string)
            {
                if (string.IsNullOrWhiteSpace((string)value)) { return false; }
            }
            else if (value is DateTime)
            {
                if (((DateTime)value) < ValidDate) { return false; }
            }
            else if (value is DateTimeOffset)
            {
                if (((DateTimeOffset)value) < ValidDate) { return false; }
            }
            else if (value is IEnumerable)
            {
                foreach (var item in (IEnumerable)value) { return true; }
                return false;
            }
            else
            {
                try
                {
                    var num = (decimal)Convert.ChangeType(value, typeof(decimal));
                    if (num == 0) { return false; }
                }
                catch (Exception) { }
            }
            return true;
        }


        ///// <summary>判斷型態是否可為 Null</summary>
        //public static bool IsNullable(Type type)
        //{
        //    if (!type.IsValueType) { return true; } /* ref-type */
        //    if (Nullable.GetUnderlyingType(type) != null) { return true; } /* Nullable<T> */
        //    return false; /* value-type */
        //}


        /// <summary>建立指定隔離層級的交易範圍。</summary>
        /// <param name="level">交易使用的隔離層級。</param>
        /// <returns>已建立的交易範圍物件。</returns>
        private static TransactionScope tx(IsolationLevel level)
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = level,
            });
        }

        /// <summary>v0: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料，且不能加入新資料。</summary>
        /// <returns>可序列化隔離層級的交易範圍。</returns>
        public static TransactionScope TxSerializable() { return tx(IsolationLevel.Serializable); }

        /// <summary>v1: 在交易期間可以讀取 Volatile (易失性)資料，但無法修改該資料。 在交易期間可以加入新資料。</summary>
        /// <returns>可重複讀取隔離層級的交易範圍。</returns>
        public static TransactionScope TxRepeatableRead() { return tx(IsolationLevel.RepeatableRead); }

        /// <summary>v2: 在交易期間無法讀取 Volatile (易失性)資料，但可以修改該資料。</summary>
        /// <returns>讀取已認可隔離層級的交易範圍。</returns>
        public static TransactionScope TxReadCommitted() { return tx(IsolationLevel.ReadCommitted); }

        /// <summary>v3: 在交易期間可以讀取和修改 Volatile (易失性)資料。[髒讀]</summary>
        /// <returns>讀取未認可隔離層級的交易範圍。</returns>
        public static TransactionScope TxReadUncommitted() { return tx(IsolationLevel.ReadUncommitted); }






        /*####################################################################*/


        /// <summary>將 enum 轉為 dictionary&lt;string,string&gt;</summary>
        public static Dictionary<string, string> EnumToDictionary<T>()
        {
            Type enumType = typeof(T);
            return EnumToDictionary(enumType);
        }

        /// <summary>將 enum 轉為 dictionary&lt;string,string&gt;</summary>
        /// <param name="enumType">要轉換的列舉型別。</param>
        public static Dictionary<string, string> EnumToDictionary(Type enumType)
        {
            if (enumType.BaseType != typeof(Enum))
            { throw new ArgumentException("T must be of type System.Enum"); }

            var dictionary = new Dictionary<string, string>();

            foreach (var name in Enum.GetNames(enumType))
            {
                var desc = enumType.GetField(name).GetDisplayName();
                dictionary.Add(name, desc ?? name);
            }

            return dictionary;
        }



        /// <summary>取得所有 enum 的值</summary>
        public static T[] GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }


        /// <summary>迭代指定範圍，可以 [小到大] 或 [大到小]</summary>
        /// <param name="start">起始整數（包含）。</param>
        /// <param name="end">結束整數（包含）。</param>
        public static IEnumerable<int> EnumerateRange(int start, int end)
        {
            if (start < end)
            { for (int i = start; i <= end; i++) { yield return i; } }
            else
            { for (int i = start; i >= end; i--) { yield return i; } }
        }


        /// <summary>迭代指定範圍，可以 [小到大] 或 [大到小]</summary>
        /// <param name="start">起始日期（包含）。</param>
        /// <param name="end">結束日期（包含）。</param>
        public static IEnumerable<DateTime> EnumerateRange(DateTime start, DateTime end)
        {
            if (start < end)
            { for (DateTime i = start; i <= end; i = i.AddDays(1)) { yield return i; } }
            else
            { for (DateTime i = start; i >= end; i = i.AddDays(-1)) { yield return i; } }
        }





         



        /*####################################################################*/

        /// <summary>取得太陽日</summary>
        /// <param name="date">日期時間值。</param>
        public static int? GetSolarDay(DateTime? date)
        {
            if (date == null) { return null; }

            int year = date.Value.Year - 1900;
            return year * 1000 + date.Value.DayOfYear;
        }



        /// <summary>解析太陽日</summary>
        /// <param name="solarDay">太陽日值（格式為 `年差*1000 + 當年第幾天`）。</param>
        public static DateTime? ParseSolarDay(int? solarDay)
        {
            if (solarDay == null) { return null; }

            int year = solarDay.Value / 1000 + 1900;
            int day = solarDay.Value % 1000;
            return new DateTime(year, 1, 1).AddDays(day - 1);
        }



        /// <summary>解析民國日期</summary>
        /// <param name="dateStr">民國日期字串（例如 `112/08/15`）。</param>
        public static DateTime? ParseCnDate(string dateStr)
        {
            if (dateStr == null) { return null; }

            Match m = Regex.Match(dateStr.Trim(), @"^(\d+)\D+(\d+)\D+(\d+)");
            if (!m.Success) { return null; }

            int year = int.Parse(m.Groups[1].Value) + 1911;
            int month = int.Parse(m.Groups[2].Value);
            int day = int.Parse(m.Groups[3].Value);

            return new DateTime(year, month, day);
        }


        /// <summary>解析民國日期</summary>
        /// <param name="dateStr">民國日期時間字串（例如 `112/08/15 13:20:30`）。</param>
        public static DateTime? ParseCnDateTime(string dateStr)
        {
            if (dateStr == null) { return null; }

            Match m = Regex.Match(dateStr.Trim(), @"^(\d+)\D+(\d+)\D+(\d+)\D+(\d+)\D+(\d+)\D+(\d+)");
            if (!m.Success) { return null; }

            int year = int.Parse(m.Groups[1].Value) + 1911;
            int month = int.Parse(m.Groups[2].Value);
            int day = int.Parse(m.Groups[3].Value);
            int hour = int.Parse(m.Groups[4].Value);
            int minute = int.Parse(m.Groups[5].Value);
            int second = int.Parse(m.Groups[6].Value);

            return new DateTime(year, month, day, hour, minute, second);
        }






        /*####################################################################*/

        /// <summary>傳回兩個日期中較小的一個</summary>
        /// <param name="val1">第一個日期值。</param>
        /// <param name="val2">第二個日期值。</param>
        public static DateTime Min(DateTime val1, DateTime val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        /// <summary>傳回兩個日期中較大的一個</summary>
        /// <param name="val1">第一個日期值。</param>
        /// <param name="val2">第二個日期值。</param>
        public static DateTime Max(DateTime val1, DateTime val2)
        {
            return val1 > val2 ? val1 : val2;
        }


        /// <summary>傳回兩個日期中較小的一個</summary>
        /// <param name="val1">第一個日期時間偏移值。</param>
        /// <param name="val2">第二個日期時間偏移值。</param>
        public static DateTimeOffset Min(DateTimeOffset val1, DateTimeOffset val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        /// <summary>傳回兩個日期中較大的一個</summary>
        /// <param name="val1">第一個日期時間偏移值。</param>
        /// <param name="val2">第二個日期時間偏移值。</param>
        public static DateTimeOffset Max(DateTimeOffset val1, DateTimeOffset val2)
        {
            return val1 > val2 ? val1 : val2;
        }


        /// <summary>傳回兩個時間中較小的一個</summary>
        /// <param name="val1">第一個時間值。</param>
        /// <param name="val2">第二個時間值。</param>
        public static TimeSpan Min(TimeSpan val1, TimeSpan val2)
        {
            return val1 < val2 ? val1 : val2;
        }

        /// <summary>傳回兩個時間中較大的一個</summary>
        /// <param name="val1">第一個時間值。</param>
        /// <param name="val2">第二個時間值。</param>
        public static TimeSpan Max(TimeSpan val1, TimeSpan val2)
        {
            return val1 > val2 ? val1 : val2;
        }




        /*####################################################################*/
        /// <summary>讀取旗標檔中的 Process Id。</summary>
        /// <param name="flagFile">旗標檔完整路徑。</param>
        /// <returns>成功時回傳 Process Id，失敗時回傳 -1。</returns>
        private static int readFlagId(string flagFile)
        {
            if (!File.Exists(flagFile)) { return -1; }

            string pidStr = "-1";
            using (var file = File.Open(flagFile, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(file))
            {
                pidStr = sr.ReadToEnd();
            }

            int pid;
            if (!int.TryParse(pidStr, out pid)) { return -1; }

            return pid;
        }

        /// <summary>寫入 Process Id 到旗標檔。</summary>
        /// <param name="flagFile">旗標檔完整路徑。</param>
        /// <param name="id">要寫入的 Process Id。</param>
        private static void writeFlagId(string flagFile, int id)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(flagFile));

            using (var file = File.Open(flagFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            using (var sw = new StreamWriter(file))
            {
                sw.Write(id.ToString());
            }
        }



        /// <summary>是否鎖定中 Process Id</summary>
        /// <param name="pidFile">用來記錄 Process Id 的旗標檔路徑。</param>
        public static bool IsLockedProcessId(string pidFile)
        {
            int pid = readFlagId(pidFile);
            string name = Process.GetCurrentProcess().ProcessName;
            return Process.GetProcesses().Any(x => x.Id == pid && x.ProcessName == name);
        }


        /// <summary>鎖定 Process Id</summary>
        /// <param name="pidFile">用來記錄 Process Id 的旗標檔路徑。</param>
        public static bool LockProcessId(string pidFile)
        {
            if (IsLockedProcessId(pidFile)) { return false; }

            writeFlagId(pidFile, Process.GetCurrentProcess().Id);
            return true;
        }


        /// <summary>解鎖 Process Id</summary>
        /// <param name="pidFile">用來記錄 Process Id 的旗標檔路徑。</param>
        public static bool UnlockProcessId(string pidFile)
        {
            int pid = readFlagId(pidFile);
            if (pid != Process.GetCurrentProcess().Id) { return false; }

            writeFlagId(pidFile, -1);
            return true;
        }






        /*####################################################################*/

        /// <summary>將位元組陣列轉為不含分隔符號的十六進位字串。</summary>
        /// <param name="byteArray">要轉換的位元組陣列。</param>
        /// <returns>十六進位字串。</returns>
        private static string getByteArrayString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray).Replace("-", "");
        }

        /// <summary>MD5 輸入字串</summary>
        /// <param name="value">要計算 MD5 的字串內容。</param>
        public static string Md5String(string value)
        {
            byte[] bytes = Encoding.Default.GetBytes(value);
            byte[] hash = MD5.Create().ComputeHash(bytes);
            return getByteArrayString(hash);
        }

        /// <summary>MD5 檔案</summary>
        /// <param name="filePath">檔案路徑。</param>
        public static string Md5File(string filePath)
        {
            return Md5File(new FileStream(filePath, FileMode.Open));
        }

        /// <summary>MD5 檔案</summary>
        /// <param name="fileStream">要計算 MD5 的檔案資料流。</param>
        public static string Md5File(Stream fileStream)
        {
            byte[] hash = MD5.Create().ComputeHash(fileStream);
            return getByteArrayString(hash);
        }

        /// <summary>MD5 檔案</summary>
        /// <param name="fileContents">要計算 MD5 的檔案位元組內容。</param>
        public static string Md5File(byte[] fileContents)
        {
            byte[] hash = MD5.Create().ComputeHash(fileContents);
            return getByteArrayString(hash);
        }


        /// <summary>分配雜湊路徑 /ed/a0/123</summary>
        /// <param name="hashPrefix">用來參與雜湊計算的前綴字串。</param>
        /// <param name="fileId">檔案識別碼。</param>
        public static string AllotHashPath(string hashPrefix, int fileId)
        {
            byte[] bytes = Encoding.Default.GetBytes(hashPrefix + "_" + fileId);
            byte[] hash = MD5.Create().ComputeHash(bytes);
            string hashStr = BitConverter.ToString(hash).ToLower();
            return "/" + string.Join("/", hashStr.Split('-').Take(2)) + "/" + fileId;
        }



        /// <summary>分配路徑 /000/000/123</summary>
        /// <param name="fileId">檔案識別碼。</param>
        public static string AllotPath(int fileId)
        {
            string pad = fileId.ToString().PadLeft(9, '0');
            return $@"/{pad.Substring(0, 3)}/{pad.Substring(3, 3)}/{fileId}";
        }


    }
}
