using System;
using System.Collections.Concurrent;
using System.Web;

namespace OrionCore.API.Extensions
{
    /// <summary></summary>
    public static class NumberCommaExtensions
    {
        private static readonly ConcurrentDictionary<int, string> _formats = new ConcurrentDictionary<int, string>();

        private static string getFormat(int digits)
        {
            return _formats.GetOrAdd(digits, x =>
            {
                if (digits == 0) { return "{0:#,##0}"; }

                string digitFmt = "";
                if (digits == -1)
                { digitFmt  = new string('#',10); }
                else if (digits > 0)
                { digitFmt = "0".PadLeft(digits, '#'); }

                return "{0:#,##0." + digitFmt + "}";
            });
        }


        /// <summary></summary>
        private static string comma(object value)
        {
            if (value == null) { return null; }

            var fmt = getFormat(-1);
            return string.Format(fmt, value);
        }


        /// <summary></summary>
        public static string Comma(this short value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this ushort value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this int value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this uint value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this long value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this ulong value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this float value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this double value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this decimal value) { return comma(value); }


        /// <summary></summary>
        public static string Comma(this short? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this ushort? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this int? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this uint? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this long? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this ulong? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this float? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this double? value) { return comma(value); }
        /// <summary></summary>
        public static string Comma(this decimal? value) { return comma(value); }


 

        /// <summary></summary>
        public static string Comma(this float value, int digits)
        {
            return Comma((double)value, digits);
        }
        /// <summary></summary>
        public static string Comma(this double value, int digits)
        {
            if (digits < 0) { throw new ArgumentOutOfRangeException(nameof(digits), "進位數不可以小於0"); }

            value = Math.Round(value, digits);

            var fmt = getFormat(digits);
            return string.Format(fmt, value);
        }
        /// <summary></summary>
        public static string Comma(this decimal value, int digits)
        {
            if (digits < 0) { throw new ArgumentOutOfRangeException(nameof(digits), "進位數不可以小於0"); }

            value = Math.Round(value, digits);

            var fmt = getFormat(digits);
            return string.Format(fmt, value);
        }


        /// <summary></summary>
        public static string Comma(this float? value, int digits)
        {
            if (value == null) { return null; }
            return Comma((double)value.Value, digits);
        }
        /// <summary></summary>
        public static string Comma(this double? value, int digits)
        {
            if (value == null) { return null; }
            return Comma(value.Value, digits);
        }
        /// <summary></summary>
        public static string Comma(this decimal? value, int digits)
        {
            if (value == null) { return null; }
            return Comma(value.Value, digits);
        }




    }
}
