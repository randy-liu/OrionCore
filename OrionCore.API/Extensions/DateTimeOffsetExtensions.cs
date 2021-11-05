using System;

namespace OrionCore.API.Extensions
{
    /// <summary>定義 DateTimeOffset 的 Extension</summary>
    public static class DateTimeOffsetExtensions
    {

        /// <summary>修補 DateTimeOffset 的時區到當前時區</summary>
        public static DateTimeOffset PatchZone(this DateTimeOffset value, TimeZoneInfo zone)
        {
            if (zone == null || value.Offset == zone.BaseUtcOffset) { return value; }

            TimeSpan diff = value.Offset - zone.BaseUtcOffset;
            value = TimeZoneInfo.ConvertTime(value, zone).Add(diff);
            return value;
        }


        /// <summary>修補 DateTimeOffset 的時區到當前時區</summary>
        public static DateTimeOffset? PatchZone(this DateTimeOffset? value, TimeZoneInfo zone)
        {
            if (value == null) { return value; }
            return PatchZone(value.Value, zone);
        }


        /// <summary>轉換 DateTimeOffset 到當前時區</summary>
        public static DateTimeOffset ConvertZone(this DateTimeOffset value, TimeZoneInfo zone)
        {
            if (zone == null || value.Offset == zone.BaseUtcOffset) { return value; }

            value = TimeZoneInfo.ConvertTime(value, zone);
            return value;
        }


        /// <summary>轉換 DateTimeOffset 到當前時區</summary>
        public static DateTimeOffset? ConvertZone(this DateTimeOffset? value, TimeZoneInfo zone)
        {
            if (value == null) { return value; }
            return ConvertZone(value.Value, zone);
        }


	}
}
