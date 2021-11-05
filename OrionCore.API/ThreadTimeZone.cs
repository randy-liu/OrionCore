using OrionCore.API.Extensions;
using System;

namespace OrionCore.API
{
    /// <summary>Thread 的時區</summary>
    public static class ThreadTimeZone
    {
        [ThreadStatic] static TimeZoneInfo _current;


        /// <summary>當前 Thread 的時區</summary>
        public static TimeZoneInfo Current
        {
            get { return _current; }
            set { _current = value; }
        }


        /// <summary>修補 DateTimeOffset 的時區到當前時區</summary>
        public static DateTimeOffset PatchZone(DateTimeOffset value)
        {
            return value.PatchZone(_current);
        }

        /// <summary>修補 DateTimeOffset 的時區到當前時區</summary>
        public static DateTimeOffset? PatchZone(DateTimeOffset? value)
        {
            return value.PatchZone(_current);
        }


        /// <summary>轉換 DateTimeOffset 到當前時區</summary>
        public static DateTimeOffset ConvertZone(DateTimeOffset value)
        {
            return value.ConvertZone(_current);
        }


        /// <summary>轉換 DateTimeOffset 到當前時區</summary>
        public static DateTimeOffset? ConvertZone(DateTimeOffset? value)
        {
            return value.ConvertZone(_current);
        }


    }

}
