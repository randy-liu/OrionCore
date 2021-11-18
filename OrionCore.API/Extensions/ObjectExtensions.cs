using System;
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
            if(obj is DataRow row) 
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in row.Table.Columns)
                { dict[col.ColumnName] = row[col]; }

                return dict;
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

        public static string Comma(this object value)
        {
            if (value == null) { return null; }

            var valueSplit = value.ToString().Split('.');
            valueSplit[0] = Regex.Replace(valueSplit[0], @"(\d)(?=(\d{3})+(?!\d))", "$1,");

            if (valueSplit.Length > 1) { valueSplit[1] = valueSplit[1].TrimEnd('0'); }

            return string.Join(".", valueSplit);
        }




        public static string ShowDate(this object value)
        {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is DateTime date) { return date.ToString("yyyy-MM-dd"); }

            return Regex.Replace(value.ToString(), @"(\d{4})(\d{2})(\d{2})", "$1-$2-$3");
        }

        public static string ShowDateTime(this object value)
        {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is DateTime date) { return date.ToString("yyyy-MM-dd HH:mm:ss"); }

            return Regex.Replace(value.ToString(), @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})", "$1-$2-$3 $4:$5:$6");
        }


        public static string ShowTime(this object value)
        {
            if (value == null) { return null; }
            if (value is DBNull) { return null; }
            if (value is DateTime date) { return date.ToString("HH:mm"); }

            string str = value.ToString().PadLeft(4, '0');
            return Regex.Replace(str, @"(\d{2})(\d{2})", "$1:$2");
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
                    if (value is DateTime) { return value; }
                    if (formats.Length == 0) { formats = new[] { "yyyyMMdd", "yyyy-MM-dd" }; }
                    return DateTime.ParseExact(value.ToString(), formats, null);
                }
                else if (type == typeof(TimeSpan))
                {
                    if (value is TimeSpan) { return value; }
                    if (formats.Length == 0) { formats = new[] { "hmm", "hhmm", "hh:mm" }; }
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