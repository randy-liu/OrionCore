using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Orion.Api.Extensions
{
    /// <summary>定義 Object 的 Extension</summary>
    public static class ObjectExtensions
    {
        private static readonly List<JsonConverter> _defaultConverters = new List<JsonConverter>()
        {
            new StringEnumConverter(),
            new IsoDateTimeConverter(),
			//new StringEnumConverter() { CamelCaseText = true },
		};



        /// <summary>JSON 序列化前的資料預處理。</summary>
        /// <param name="obj">待處理物件。</param>
        /// <returns>可直接序列化的物件內容。</returns>
        private static object preJson(this object obj)
        {
            if (obj is DataTable table)
            {
                return table.ToList(r => r.ToDictionary());
            }
            if (obj is DataRow row) 
            {
                return row.ToDictionary(); 
            }

            return obj;
        }


        /// <summary>將物件序列化為 JSON 字串。</summary>
        /// <param name="obj">來源物件。</param>
        /// <returns>序列化後的 JSON 字串。</returns>
        public static string ToJson(this object obj)
        {
            obj = preJson(obj);

            var settings = new JsonSerializerSettings()
            {
                Converters = _defaultConverters,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            string result = JsonConvert.SerializeObject(obj, (Type)null, settings);
            return result;
        }


        /// <summary>將物件序列化為縮排格式的 JSON 字串。</summary>
        /// <param name="obj">來源物件。</param>
        /// <returns>格式化後的 JSON 字串。</returns>
        public static string ToFormatJson(this object obj)
        {
            obj = preJson(obj);

            var settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                Converters = _defaultConverters,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            string result = JsonConvert.SerializeObject(obj, (Type)null, settings);
            return result;
        }



        /// <summary>將 JSON 字串反序列化為指定型別物件。</summary>
        /// <typeparam name="TObject">目標型別。</typeparam>
        /// <param name="jsonStr">JSON 字串。</param>
        /// <returns>反序列化結果；輸入為 `null` 時回傳型別預設值。</returns>
        public static TObject JsonToObject<TObject>(this string jsonStr)
        {
            if (jsonStr == null) { return default(TObject); }

            var result = JsonConvert.DeserializeObject<TObject>(jsonStr);
            return result;
        }









        /*####################################################################*/

        private static readonly ConcurrentDictionary<int, string> _formats = new ConcurrentDictionary<int, string>();

        private static string getFormat(int digits)
        {
            return _formats.GetOrAdd(digits, x =>
            {
                if (digits == 0) { return "{0:#,##0}"; }

                string digitFmt = "";
                if (digits == -1)
                { digitFmt = new string('#', 10); }
                else if (digits > 0)
                { digitFmt = "0".PadLeft(digits, '#'); }

                return "{0:#,##0." + digitFmt + "}";
            });
        }


		/// <summary>將數值格式化為千分位字串（自動小數位）。</summary>
		/// <param name="value">欄位值。</param>
        /// <typeparam name="T">數值型別。</typeparam>
        /// <returns>千分位格式字串。</returns>
        public static string Comma<T>(this T value) where T : struct
        {
            var fmt = getFormat(-1);
            return string.Format(fmt, value);
        }
		/// <summary>將可空數值格式化為千分位字串（自動小數位）。</summary>
		/// <param name="value">欄位值。</param>
        /// <typeparam name="T">數值型別。</typeparam>
        /// <returns>千分位格式字串；輸入為 `null` 時回傳 `null`。</returns>
        public static string Comma<T>(this T? value) where T : struct
        {
            if (value == null) { return null; }
            return Comma(value.Value);
        }


		/// <summary>將數值格式化為千分位字串（指定小數位）。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="digits">小數位數。</param>
        /// <typeparam name="T">數值型別。</typeparam>
        /// <returns>指定小數位的千分位格式字串。</returns>
        public static string Comma<T>(this T value, int digits) where T : struct
        {
            if (digits < 0) { throw new ArgumentOutOfRangeException(nameof(digits), "進位數不可以小於0"); }

            decimal num = Convert.ToDecimal(value);
            num = Math.Round(num, digits);

            var fmt = getFormat(digits);
            return string.Format(fmt, num);
        }
		/// <summary>將可空數值格式化為千分位字串（指定小數位）。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="digits">小數位數。</param>
        /// <typeparam name="T">數值型別。</typeparam>
        /// <returns>指定小數位的千分位格式字串；輸入為 `null` 時回傳 `null`。</returns>
        public static string Comma<T>(this T? value, int digits) where T : struct
        {
            if (value == null) { return null; }
            return Comma(value.Value, digits);
        }







        /*####################################################################*/

        /// <summary>將物件轉為指定型別，失敗時回傳型別預設值。</summary>
        /// <typeparam name="T">目標型別。</typeparam>
        /// <param name="value">欄位值。</param>
        /// <param name="formats">格式字串集合。</param>
        /// <returns>轉型後結果；失敗時為型別預設值。</returns>
        public static T ConvertTo<T>(this object value, params string[] formats)
        {
            Type type = typeof(T);
            object result = ConvertTo(value, type, formats);
            if (result == null && type.IsValueType) { result = Activator.CreateInstance(type); }
            return (T)result;
        }



        /// <summary>依指定型別將物件轉型，失敗時回傳 `null`。</summary>
        /// <param name="value">欄位值。</param>
        /// <param name="type">型別。</param>
        /// <param name="formats">格式字串集合。</param>
        /// <returns>轉型後物件；失敗時回傳 `null`。</returns>
        public static object ConvertTo(this object value, Type type, params string[] formats)
        {
            if (value == null) { return null; }
            if (value.GetType() == type) { return value; }

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
                    if (formats.Length > 0)
                    { return Guid.ParseExact(value.ToString(), formats[0]); }
                    else
                    { return Guid.Parse(value.ToString()); }
                }
                else if (type == typeof(DateTime))
                {
                    if (formats.Length == 0) { formats = new[] { "yyyyMMdd", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" }; }
                    return DateTime.ParseExact(value.ToString(), formats, null);
                }
                else if (type == typeof(DateTimeOffset))
                {
                    var result = DateTimeOffset.Parse(value.ToString());
                    result = ThreadTimeZone.PatchZone(result);
                    return result;
                }
                else if (type == typeof(TimeSpan))
                {
                    if (formats.Length == 0) { formats = new[] { "hmm", "hhmm", "hh\\:mm", "hh\\:mm\\:ss" }; }
                    return TimeSpan.ParseExact(value.ToString(), formats, null);
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





        /*############################################################################*/

		/// <summary>將匿名物件屬性轉為字典。</summary>
        /// <param name="anonymousObj">匿名物件。</param>
        /// <returns>屬性名稱與值的字典。</returns>
		public static Dictionary<string, object> AnonymousToDictionary(this object anonymousObj)
        {
            var result = new Dictionary<string, object>();
            if (anonymousObj == null) { return result; }

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(anonymousObj))
            {
                result.Add(prop.Name, prop.GetValue(anonymousObj));
            }
            return result;
        }




    }
}
