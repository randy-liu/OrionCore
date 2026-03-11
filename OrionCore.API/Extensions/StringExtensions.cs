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

        /// <summary>判斷字串是否包含非空白文字。</summary>
        /// <param name="s">要檢查是否為非空白字串的文字。</param>
        /// <returns>字串包含非空白文字時回傳 `true`。</returns>
        public static bool HasText(this String s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>判斷字串是否為 `null`、空字串或僅含空白。</summary>
        /// <param name="s">要檢查是否為空白字串的文字。</param>
        /// <returns>字串為空白時回傳 `true`。</returns>
        public static bool NoText(this String s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>以忽略大小寫模式判斷字串是否符合正規表達式。</summary>
        /// <param name="input">要進行比對的輸入字串。</param>
        /// <param name="pattern">正規表達式模式，忽略大小寫。</param>
        /// <returns>符合模式時回傳 `true`；輸入或模式為空白時回傳 `false`。</returns>
        public static bool IsMatch(this String input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }


        /// <summary>以指定選項判斷字串是否符合正規表達式。</summary>
        /// <param name="input">要進行比對的輸入字串。</param>
        /// <param name="pattern">正規表達式模式。</param>
        /// <param name="option">正規表達式比對選項。</param>
        /// <returns>符合模式時回傳 `true`；輸入或模式為空白時回傳 `false`。</returns>
        public static bool IsMatch(this String input, string pattern, RegexOptions option)
        {
            if (!HasText(input) || !HasText(pattern)) { return false; }
            return Regex.IsMatch(input, pattern, option);
        }




        /*#############################################################*/




        /// <summary>限制字串長度，超過上限時截斷。</summary>
        /// <param name="input">要限制長度的輸入字串。</param>
        /// <param name="limit">字串最大長度。</param>
        /// <returns>原字串或截斷後字串。</returns>
        public static string LimitLength(this string input, int limit)
        {
            if (input == null || input.Length <= limit) { return input; }
            return input.Substring(0, limit);
        }


        /// <summary>重複字串指定次數並直接串接。</summary>
        /// <param name="source">要重複輸出的字串內容。</param>
        /// <param name="count">重複次數。</param>
        /// <returns>重複後的字串。</returns>
        public static string Repeat(this string source, int count)
        {
            return Repeat(source, count, "");
        }

        /// <summary>重複字串指定次數，並以分隔字元串接。</summary>
        /// <param name="source">要重複輸出的字串內容。</param>
        /// <param name="count">重複次數。</param>
        /// <param name="separator">每次重複間的分隔字元。</param>
        /// <returns>重複後的字串。</returns>
        public static string Repeat(this string source, int count, string separator)
        {
            return Enumerable.Repeat(source, count).JoinBy(separator);
        }



        /// <summary>以不區分大小寫方式比較兩個字串是否相等。</summary>
        /// <param name="s">第一個比較字串。</param>
        /// <param name="t">要比較的另一個字串。</param>
        /// <returns>兩字串相等時回傳 `true`。</returns>
        public static bool EqualsIgnoreCase(this string s, string t)
        {
            return string.Equals(s, t, StringComparison.OrdinalIgnoreCase);
        }




        /// <summary>將字串轉為指定列舉型別，失敗時拋出例外。</summary>
        /// <param name="enumStr">要轉換為列舉的字串值。</param>
        /// <typeparam name="TEnum">目標列舉型別。</typeparam>
        /// <returns>轉換成功的列舉值。</returns>
        public static TEnum ToEnum<TEnum>(this string enumStr) where TEnum : struct, Enum
        {
            //if(enumStr == null) { throw new ArgumentException("無法轉換 null 到 Enum " + typeof(TEnum).Name); }

            bool succ = Enum.TryParse(enumStr, true, out TEnum result);
            if (succ) { return result; }
            throw new ArgumentException("無法轉換 " + enumStr + " 到 Enum " + typeof(TEnum).Name);
        }

        /// <summary>將字串轉為指定列舉型別，失敗時回傳預設值。</summary>
        /// <param name="enumStr">要轉換為列舉的字串值。</param>
        /// <param name="defaultValue">轉換失敗時回傳的預設 Enum 值。</param>
        /// <typeparam name="TEnum">目標列舉型別。</typeparam>
        /// <returns>轉換成功的列舉值，或轉換失敗時的預設值。</returns>
        public static TEnum ToEnum<TEnum>(this string enumStr, TEnum defaultValue) where TEnum : struct, Enum
        {
            try
            { return ToEnum<TEnum>(enumStr); }
            catch
            { return defaultValue; }
        }




        /*####################################################################*/

        private static ConcurrentDictionary<Type, Action<object>> _stringPropTrimMap = new ConcurrentDictionary<Type, Action<object>>();

        /// <summary>建立指定型別的字串屬性修剪動作。</summary>
        /// <param name="type">要掃描字串屬性的模型型別。</param>
        /// <returns>可將該型別所有字串屬性進行 `Trim` 的動作。</returns>
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
        /// <param name="model">模型物件。</param>
        public static void TrimStringPropertys<TModel>(this TModel model) where TModel : class
        {
            Action<object> trims = _stringPropTrimMap.GetOrAdd(typeof(TModel), makeStringPropTrim);
            trims(model);
        }





        /*####################################################################*/

        /// <summary>依指定分隔字串拆解並轉型為不重複清單。</summary>
        /// <param name="idsStr">要分割與轉型的字串內容。</param>
        /// <param name="separator">分隔字元。</param>
        /// <typeparam name="T">目標元素型別。</typeparam>
        /// <returns>轉型成功且去重後的清單。</returns>
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

        /// <summary>以逗號拆解並轉型為不重複清單。</summary>
        /// <param name="idsStr">要以逗號分隔並轉型的字串內容。</param>
        /// <typeparam name="T">目標元素型別。</typeparam>
        /// <returns>轉型成功且去重後的清單。</returns>
        public static List<T> ToIdsList<T>(this string idsStr)
        {
            return ToIdsList<T>(idsStr, ",");
        }


        /// <summary>依指定分隔字串拆解並轉型為陣列。</summary>
        /// <param name="idsStr">要分割與轉型的字串內容。</param>
        /// <param name="separator">分隔字元。</param>
        /// <typeparam name="T">目標元素型別。</typeparam>
        /// <returns>轉型成功且去重後的陣列。</returns>
        public static T[] ToIdsArray<T>(this string idsStr, string separator)
        {
            return ToIdsList<T>(idsStr, separator).ToArray();
        }

        /// <summary>以逗號拆解並轉型為陣列。</summary>
        /// <param name="idsStr">要以逗號分隔並轉型的字串內容。</param>
        /// <typeparam name="T">目標元素型別。</typeparam>
        /// <returns>轉型成功且去重後的陣列。</returns>
        public static T[] ToIdsArray<T>(this string idsStr)
        {
            return ToIdsList<T>(idsStr, ",").ToArray();
        }



    }

}
