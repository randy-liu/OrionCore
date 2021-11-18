using System;
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
        public static TEnum ToEnum<TEnum>(this string enumStr)
        {
            try
            {
                return (TEnum)Enum.Parse(typeof(TEnum), enumStr);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("無法轉換 " + enumStr + " 到 Enum " + typeof(TEnum).Name, ex);
            }
        }

        /// <summary>將 string 轉換為 Enum，若失敗則回傳指定 Default Enum</summary>
        public static TEnum ToEnum<TEnum>(this string enumStr, TEnum defaultValue)
        {
            try
            {
                return ToEnum<TEnum>(enumStr);
            }
            catch
            {
                return defaultValue;
            }
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