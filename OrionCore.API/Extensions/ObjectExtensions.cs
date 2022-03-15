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



        /// <summary>Json 轉換的前置處裡 </summary>
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


        /// <summary>將 object 轉換為 Json</summary>
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


        /// <summary>將 object 轉換為縮排格式化的 Json</summary>
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



        /// <summary>將 json string 轉換為 Object</summary>
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


        /// <summary></summary>
        public static string Comma<T>(this T value) where T : struct
        {
            var fmt = getFormat(-1);
            return string.Format(fmt, value);
        }
        /// <summary></summary>
        public static string Comma<T>(this T? value) where T : struct
        {
            if (value == null) { return null; }
            return Comma(value.Value);
        }


        /// <summary></summary>
        public static string Comma<T>(this T value, int digits) where T : struct
        {
            if (digits < 0) { throw new ArgumentOutOfRangeException(nameof(digits), "進位數不可以小於0"); }

            decimal num = Convert.ToDecimal(value);
            num = Math.Round(num, digits);

            var fmt = getFormat(digits);
            return string.Format(fmt, num);
        }
        /// <summary></summary>
        public static string Comma<T>(this T? value, int digits) where T : struct
        {
            if (value == null) { return null; }
            return Comma(value.Value, digits);
        }







        /*####################################################################*/

        /// <summary>根據 T 將 value 轉型，若轉型失敗則回傳預設值</summary>
        public static T ConvertTo<T>(this object value, params string[] formats)
        {
            Type type = typeof(T);
            object result = ConvertTo(value, type, formats);
            if (result == null && type.IsValueType) { result = Activator.CreateInstance(type); }
            return (T)result;
        }



        /// <summary>根據 type 將 value 轉型，若轉型失敗則回傳 null</summary>
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

		/// <summary>將匿名物件轉換成 Dictionary</summary>
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