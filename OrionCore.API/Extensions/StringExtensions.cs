using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Orion.Api.Extensions
{

    /// <summary>定義 String 的 Extension</summary>
    public static class StringExtensions
    {

        /// <summary>檢查 string 是否有文字</summary>
        public static bool HasText(this String s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>檢查 string 是否沒有文字</summary>
        public static bool NoText(this String s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>Regex IsMatch 忽略大小寫</summary>
        public static bool IsMatch(this String input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }


        /// <summary>Regex IsMatch</summary>
        public static bool IsMatch(this String input, string pattern, RegexOptions option)
        {
            if (!HasText(input) || !HasText(pattern)) { return false; }
            return Regex.IsMatch(input, pattern, option);
        }




        /*#############################################################*/




        /// <summary>限制字串長度</summary>
        public static string LimitLength(this string input, int limit)
        {
            if (input == null || input.Length <= limit) { return input; }
            return input.Substring(0, limit);
        }


        /// <summary>字串重複</summary>
        public static string Repeat(this string source, int count)
        {
            return Repeat(source, count, "");
        }

        /// <summary>字串重複</summary>
        public static string Repeat(this string source, int count, string separator)
        {
            return Enumerable.Repeat(source, count).JoinBy(separator);
        }



        /// <summary>將 string 忽略大小寫後比較是否相等</summary>
        public static bool EqualsIgnoreCase(this string s, string t)
        {
            return string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
        }




        /// <summary>將 string 轉換為 Enum，若失敗則拋出 Exception</summary>
        public static TEnum ToEnum<TEnum>(this string enumStr) where TEnum : struct, Enum
        {
            //if(enumStr == null) { throw new ArgumentException("無法轉換 null 到 Enum " + typeof(TEnum).Name); }

            bool succ = Enum.TryParse(enumStr, true, out TEnum result);
            if (succ) { return result; }
            throw new ArgumentException("無法轉換 " + enumStr + " 到 Enum " + typeof(TEnum).Name);
        }

        /// <summary>將 string 轉換為 Enum，若失敗則回傳指定 Default Enum</summary>
        public static TEnum ToEnum<TEnum>(this string enumStr, TEnum defaultValue) where TEnum : struct, Enum
        {
            try
            { return ToEnum<TEnum>(enumStr); }
            catch
            { return defaultValue; }
        }




        /*####################################################################*/

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
        public static void TrimStringPropertys<TModel>(this TModel model) where TModel : class
        {
            Action<object> trims = _stringPropTrimMap.GetOrAdd(typeof(TModel), makeStringPropTrim);
            trims(model);
        }





        /*####################################################################*/

        /// <summary>將字串根據,分隔轉為List&lt;T&gt; 若轉型失敗則拋棄</summary>
        public static List<T> ToIdsList<T>(this string idsStr, string separator)
        {
            if (string.IsNullOrWhiteSpace(idsStr)) { return new List<T>(); }

            Type type = typeof(T);

            List<T> list = idsStr
                .Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ConvertTo(type))
                .Where(x => x != null)
                .Cast<T>()
                .Distinct()
                .ToList();

            return list;
        }

        /// <summary>將字串根據,分隔轉為List&lt;T&gt; 若轉型失敗則拋棄</summary>
        public static List<T> ToIdsList<T>(this string idsStr)
        {
            return ToIdsList<T>(idsStr, ",");
        }


        /// <summary>將字串根據,分隔轉為 T[] 若轉型失敗則拋棄</summary>
        public static T[] ToIdsArray<T>(this string idsStr, string separator)
        {
            return ToIdsList<T>(idsStr, separator).ToArray();
        }

        /// <summary>將字串根據,分隔轉為 T[] 若轉型失敗則拋棄</summary>
        public static T[] ToIdsArray<T>(this string idsStr)
        {
            return ToIdsList<T>(idsStr, ",").ToArray();
        }



    }

}