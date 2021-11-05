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
using System.Threading;
using System.Transactions;
using OrionCore.API.Extensions;

namespace OrionCore.API
{
	/// <summary>Utility Tools</summary>
	public static class OrionUtils
	{


		/// <summary></summary>
		public static readonly DateTime ValidDate = new DateTime(1753, 1, 1);



		/// <summary>判斷 value( Enum, string, DateTime, DateTimeOffset, IEnumerable, decimal ) 是否有值 </summary>
		public static bool HasValue(object value)
		{
			if (value == null) { return false; }
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


		/// <summary>判斷型態是否可為 Null</summary>
		public static bool IsNullable(Type type)
		{
			if (!type.IsValueType) { return true; } /* ref-type */
			if (Nullable.GetUnderlyingType(type) != null) { return true; } /* Nullable<T> */
			return false; /* value-type */
		}



		/// <summary>根據 T 將 value 轉型，若轉型失敗則回傳預設值</summary>
		public static T ConvertType<T>(object value)
		{
			Type type = typeof(T);
			object result = ConvertType(value, type);
			if (result == null && type.IsValueType) { result = Activator.CreateInstance(type); }
			return (T)result;
		}


		/// <summary>根據 type 將 value 轉型，若轉型失敗則回傳 null</summary>
		public static object ConvertType(object value, Type type)
		{
			if(value == null) { return null; }

			try
			{
				if (type == typeof(string)) { return value.ToString(); }

				var nullType = Nullable.GetUnderlyingType(type);
				if (nullType != null) { type = nullType; }

				if (type.IsEnum)
				{
					return Enum.Parse(type, value.ToString());
				}
				else if (type == typeof(Guid))
				{
					return Guid.Parse(value.ToString());
				}
				else if (type == typeof(TimeSpan))
				{
					return TimeSpan.Parse(value.ToString());
				}
				else if (type == typeof(DateTimeOffset))
				{
					var result = DateTimeOffset.Parse(value.ToString());
					result = ThreadTimeZone.PatchZone(result);
					return result;
				}
				else
				{
					return Convert.ChangeType(value, type);
				}
			}
			catch
			{
				return null;
			}
		}




		private static ConcurrentDictionary<Type, Action<object>> _stringPropTrimMap = new ConcurrentDictionary<Type, Action<object>>();

		private static Action<object> makeStringPropTrim(Type type)
		{
			Action<object> trims = (model => { });

			var propInfos = type.GetProperties()
				.Where(x => x.CanRead && x.CanWrite)
				.Where(x => x.PropertyType == typeof(string));

			foreach (var prop in propInfos)
			{
				trims += model =>
				{
					var value = prop.GetValue(model) as string;
					if (value != null) { prop.SetValue(model, value.Trim()); }
				};
			}

			return trims;
		}


		/// <summary>將 model 中所有 string type 的 properties 去除空白</summary>
		public static void TrimStringPropertys<TModel>(TModel model) where TModel : class
		{
			Action<object> trims = _stringPropTrimMap.GetOrAdd(typeof(TModel), makeStringPropTrim);
			trims(model);
		}




		/// <summary>ReadUncommitted</summary>
		public static TransactionScope TransactionReadUncommitted()
		{
			return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
			{
				IsolationLevel = IsolationLevel.ReadUncommitted,
			});
		}



		/*####################################################################*/

		/// <summary>取得 Method | Property | Field  DisplayAttribute、DescriptionAttribute 中的字串</summary>
		public static string GetDisplayName(MemberInfo info)
		{
			if (info == null) { return null; }

			var disAttr = info.GetCustomAttribute<DisplayAttribute>();
			if (disAttr != null)
			{
				if (disAttr.ResourceType == null) { return disAttr.Name; }
				return new ResourceManager(disAttr.ResourceType).GetString(disAttr.Name) ?? disAttr.Name;
			}

			var descAttr = info.GetCustomAttribute<DescriptionAttribute>();
			if (descAttr != null) { return descAttr.Description; }

			return null;
		}



		/// <summary>取得 Method | Property | Field  DisplayAttribute 中的字串</summary>
		public static string GetEnumDisplayName(object enumValue)
		{
			if (enumValue == null) { return null; }
			var fi = enumValue.GetType().GetField(enumValue.ToString());
			if (fi == null) { return null; }

			return GetDisplayName(fi);
		}


		/// <summary>將 enum 轉為 dictionary&lt;string,string&gt;</summary>
		public static Dictionary<string, string> EnumToDictionary<T>()
		{
			Type enumType = typeof(T);
			return EnumToDictionary(enumType);
		}

		/// <summary>將 enum 轉為 dictionary&lt;string,string&gt;</summary>
		public static Dictionary<string, string> EnumToDictionary(Type enumType)
		{
			if (enumType.BaseType != typeof(Enum))
			{
				throw new ArgumentException("T must be of type System.Enum");
			}

			var dictionary = new Dictionary<string, string>();

			foreach (var name in Enum.GetNames(enumType))
			{
				var desc = GetDisplayName(enumType.GetField(name));
				if (desc == null) { desc = name; }
				dictionary.Add(name, desc);
			}

			return dictionary;
		}



		/// <summary>取得所有 enum 的值</summary>
		public static T[] GetEnumValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
		}


		/// <summary>迭代指定範圍，可以 [小到大] 或 [大到小]</summary>
		public static IEnumerable<int> EnumerateRange(int start, int end)
		{
			if (start < end)
			{ for (int i = start; i <= end; i++) { yield return i; } }
			else
			{ for (int i = start; i >= end; i--) { yield return i; } }
		}



		/*####################################################################*/

		/// <summary>取得太陽日</summary>
		public static int? GetSolarDay(DateTime? date)
		{
			if (date == null) { return null; }

			int year = date.Value.Year - 1900;
			return year * 1000 + date.Value.DayOfYear;
		}



		/// <summary>解析太陽日</summary>
		public static DateTime? ParseSolarDay(int? solarDay)
		{
			if (solarDay == null) { return null; }

			int year = solarDay.Value / 1000 + 1900;
			int day = solarDay.Value % 1000;
			return new DateTime(year, 1, 1).AddDays(day - 1);
		}



		/// <summary>解析民國日期</summary>
		public static DateTime? ParseCnDate(string dateStr)
		{
			if(dateStr == null) { return null; }

			Match m = Regex.Match(dateStr.Trim(), @"^(\d+)\D+(\d+)\D+(\d+)");
			if (!m.Success) { return null; }

			int year = int.Parse(m.Groups[1].Value) + 1911;
			int month = int.Parse(m.Groups[2].Value);
			int day = int.Parse(m.Groups[3].Value);

			return new DateTime(year, month, day);
		}


		/// <summary>解析民國日期</summary>
		public static DateTime? ParseCnDateTime(string dateStr)
		{
			if(dateStr == null) { return null; }

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
		public static DateTime Min(DateTime val1, DateTime val2)
		{
			return val1 < val2 ? val1 : val2;
		}

		/// <summary>傳回兩個日期中較大的一個</summary>
		public static DateTime Max(DateTime val1, DateTime val2)
		{
			return val1 > val2 ? val1 : val2;
		}


		/// <summary>傳回兩個日期中較小的一個</summary>
		public static DateTimeOffset Min(DateTimeOffset val1, DateTimeOffset val2)
		{
			return val1 < val2 ? val1 : val2;
		}

		/// <summary>傳回兩個日期中較大的一個</summary>
		public static DateTimeOffset Max(DateTimeOffset val1, DateTimeOffset val2)
		{
			return val1 > val2 ? val1 : val2;
		}


		/// <summary>傳回兩個時間中較小的一個</summary>
		public static TimeSpan Min(TimeSpan val1, TimeSpan val2)
		{
			return val1 < val2 ? val1 : val2;
		}

		/// <summary>傳回兩個時間中較大的一個</summary>
		public static TimeSpan Max(TimeSpan val1, TimeSpan val2)
		{
			return val1 > val2 ? val1 : val2;
		}




		/*####################################################################*/

		/// <summary>將字串根據,分隔轉為List&lt;T&gt; 若轉型失敗則拋棄</summary>
		public static List<T> ToIdsList<T>(string idsStr, string separator)
		{
			if (string.IsNullOrWhiteSpace(idsStr)) { return new List<T>(); }

			Type type = typeof(T);

            List<T> list = idsStr
                .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => ConvertType(x.Trim(), type))
                .Where(x => x != null)
                .Cast<T>()
                .Distinct()
                .ToList();

            return list;
		}

		/// <summary>將字串根據,分隔轉為List&lt;T&gt; 若轉型失敗則拋棄</summary>
		public static List<T> ToIdsList<T>(string idsStr)
		{
			return ToIdsList<T>(idsStr, ",");
		}


        /// <summary>將字串根據,分隔轉為 T[] 若轉型失敗則拋棄</summary>
        public static T[] ToIdsArray<T>(string idsStr, string separator)
        {
            return ToIdsList<T>(idsStr, separator).ToArray();
        }

        /// <summary>將字串根據,分隔轉為 T[] 若轉型失敗則拋棄</summary>
        public static T[] ToIdsArray<T>(string idsStr)
        {
            return ToIdsList<T>(idsStr, ",").ToArray();
        }

               

		/// <summary>將 泛型 IEnumerable 轉為由 , 組成的 string</summary>
		public static string ToIdsString<T>(IEnumerable<T> list, string separator)
		{
			return list == null ? string.Empty : string.Join(separator, list.Distinct());
		}

        /// <summary>將 泛型 IEnumerable 轉為由 , 組成的 string</summary>
        public static string ToIdsString<T>(IEnumerable<T> list)
		{
			return ToIdsString<T>(list, ",");
		}



		/*####################################################################*/


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
		public static bool IsLockedProcessId(string pidFile)
		{
			int pid = readFlagId(pidFile);
			string name = Process.GetCurrentProcess().ProcessName;
			return Process.GetProcesses().Any(x => x.Id == pid && x.ProcessName == name);
		}


		/// <summary>鎖定 Process Id</summary>
		public static bool LockProcessId(string pidFile)
		{
			if (IsLockedProcessId(pidFile)) { return false; }

			writeFlagId(pidFile, Process.GetCurrentProcess().Id);
			return true;
		}


		/// <summary>解鎖 Process Id</summary>
		public static bool UnlockProcessId(string pidFile)
		{
			int pid = readFlagId(pidFile);
			if (pid != Process.GetCurrentProcess().Id) { return false; }

			writeFlagId(pidFile, -1);			
			return true;
		}






		/*####################################################################*/

		private static string getByteArrayString(byte[] byteArray)
		{
			return BitConverter.ToString(byteArray).Replace("-", "");
		}

		/// <summary>MD5 輸入字串</summary>
		public static string Md5String(string value)
		{
			byte[] bytes = Encoding.Default.GetBytes(value);
			byte[] hash = MD5.Create().ComputeHash(bytes);
			return getByteArrayString(hash);
		}

		/// <summary>MD5 檔案</summary>
		public static string Md5File(string filePath)
		{
			return Md5File(new FileStream(filePath, FileMode.Open));
		}

		/// <summary>MD5 檔案</summary>
		public static string Md5File(Stream fileStream)
		{
			byte[] hash = MD5.Create().ComputeHash(fileStream);
			return getByteArrayString(hash);
		}

		/// <summary>MD5 檔案</summary>
		public static string Md5File(byte[] fileContents)
		{
			byte[] hash = MD5.Create().ComputeHash(fileContents);
			return getByteArrayString(hash);
		}


		/// <summary>分配雜湊路徑 /ed/a0/123</summary>
		public static string AllotHashPath(string hashPrefix, int fileId)
		{
			byte[] bytes = Encoding.Default.GetBytes(hashPrefix + "_" + fileId);
			byte[] hash = MD5.Create().ComputeHash(bytes);
			string hashStr = BitConverter.ToString(hash).ToLower();
			return "/" + string.Join("/", hashStr.Split('-').Take(2)) + "/" + fileId;
		}



	}
}