using System;
using Microsoft.AspNetCore.Html;
using OrionCore.API.Extensions;

namespace OrionCore.Mvc.Extensions
{
    public static class HtmlStringExtensions
    {

        /// <summary>將 object 轉換為 JsonRaw</summary>
        public static IHtmlContent ToJsonRaw(this object obj)
        {
            return new HtmlString(ObjectJsonExtensions.ToJson(obj));
        }



        /// <summary>將 object 轉換為縮排格式化的 JsonRaw</summary>
        public static IHtmlContent ToFormatJsonRaw(this object obj)
        {
            return new HtmlString(ObjectJsonExtensions.ToFormatJson(obj));
        }




        /*##############################################################################*/

        private static IHtmlContent wrap<T>(T value, Func<T, string> formater)
        {
            var formated = formater(value);
            return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
        }

        private static IHtmlContent wrap<T>(T value, int digits, Func<T, int, string> formater)
        {
            var formated = formater(value, digits);
            return new HtmlString($"<span raw=\"{value}\">{formated}</span>");
        }



        /// <summary></summary>
        public static IHtmlContent CommaWrap(this short value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this ushort value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this int value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this uint value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this long value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this ulong value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this float value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this double value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this decimal value) { return wrap(value, NumberCommaExtensions.Comma); }


        /// <summary></summary>
        public static IHtmlContent CommaWrap(this short? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this ushort? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this int? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this uint? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this long? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this ulong? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this float? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this double? value) { return wrap(value, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this decimal? value) { return wrap(value, NumberCommaExtensions.Comma); }


        /// <summary></summary>
        public static IHtmlContent CommaWrap(this float value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this double value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this decimal value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }

        /// <summary></summary>
        public static IHtmlContent CommaWrap(this float? value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this double? value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }
        /// <summary></summary>
        public static IHtmlContent CommaWrap(this decimal? value, int digits) { return wrap(value, digits, NumberCommaExtensions.Comma); }


    }
}
