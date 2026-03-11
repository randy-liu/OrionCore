using System;

namespace Orion.Api.Extensions
{
	/// <summary>日期時間區段 的 Extension</summary>
	public static class DateTimeExtensions
	{

		/// <summary>取得指定日期所在月份的第一天。</summary>
		/// <param name="date">日期時間值。</param>
		/// <returns>該月份第一天的日期。</returns>
		public static DateTime FirstDateOfMonth(this DateTime date) 
		{
			return new DateTime(date.Year, date.Month, 1); 
		}

		/// <summary>取得指定日期所在月份的最後一天。</summary>
		/// <param name="date">日期時間值。</param>
		/// <returns>該月份最後一天的日期。</returns>
		public static DateTime LastDateOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1); 
		}






		///*#############################################################*/

		private static string getLiveTime(TimeSpan diffTime, int diffYear)
		{
			if (diffTime.TotalSeconds < 60) { return $"{diffTime.TotalSeconds:0} 秒前"; }
			if (diffTime.TotalMinutes < 60) { return $"{diffTime.TotalMinutes:0} 分鐘前"; }
			if (diffTime.TotalHours < 24) { return $"{diffTime.TotalHours:0} 小時前"; }
			if (diffTime.TotalDays < 30) { return $"{diffTime.TotalDays:0} 天前"; }
			if (diffTime.TotalDays < 360) { return $"{(diffTime.TotalDays / 30):0} 個月前"; }

			return diffYear + " 年前";
		}

		/// <summary>將時間轉為「幾秒前／幾分鐘前」等相對時間字串。</summary>
		/// <param name="datatime">要換算的時間。</param>
		/// <returns>相對於目前時間的描述字串。</returns>
		public static string ToLiveTime(this DateTime datatime)
		{
			string liveTime = getLiveTime(
				DateTime.Now - datatime,
				DateTime.Now.Year - datatime.Year
			);
			return liveTime;
		}
		/// <summary>將可空時間轉為相對時間字串。</summary>
		/// <param name="datatime">要換算的可空時間。</param>
		/// <returns>相對時間描述；輸入為 `null` 時回傳 `null`。</returns>
		public static string ToLiveTime(this DateTime? datatime)
		{
			if (datatime == null) { return null; }
			return ToLiveTime(datatime.Value);
		}









		/*####################################################################*/

		/// <summary>將 `DateTimeOffset` 轉為相對時間字串。</summary>
		/// <param name="datatime">要換算的時間。</param>
		/// <returns>相對於目前時間的描述字串。</returns>
		public static string ToLiveTime(this DateTimeOffset datatime)
		{
			string liveTime = getLiveTime(
				DateTimeOffset.Now - datatime,
				DateTimeOffset.Now.Year - datatime.Year
			);
			return liveTime;
		}

		/// <summary>將可空 `DateTimeOffset` 轉為相對時間字串。</summary>
		/// <param name="datatime">要換算的可空時間。</param>
		/// <returns>相對時間描述；輸入為 `null` 時回傳 `null`。</returns>
		public static string ToLiveTime(this DateTimeOffset? datatime)
		{
			if (datatime == null) { return null; }
			return ToLiveTime(datatime.Value);
		}





		/*#############################################################*/

		/// <summary>將日期格式化為短日期字串。</summary>
		/// <param name="date">日期。</param>
		/// <returns>短日期字串。</returns>
		public static string ShowDate(this DateTime date)
		{
			return date.ToString("d");
			//return date.ToString("yyyy-MM-dd");
		}
		/// <summary>將日期格式化為日期時間字串。</summary>
		/// <param name="date">日期。</param>
		/// <returns>日期時間字串。</returns>
		public static string ShowDateTime(this DateTime date)
		{
			return date.ToString();
			//return date.ToString("yyyy-MM-dd HH:mm:ss");
		}
		/// <summary>將日期格式化為短時間字串。</summary>
		/// <param name="date">日期。</param>
		/// <returns>時間字串。</returns>
		public static string ShowTime(this DateTime date)
		{
			return date.ToString("t");
			//return date.ToString("HH:mm:ss");
		}

		/// <summary>將可空日期格式化為短日期字串。</summary>
		/// <param name="date">可空日期。</param>
		/// <returns>短日期字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowDate(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowDate(date.Value);
		}
		/// <summary>將可空日期格式化為日期時間字串。</summary>
		/// <param name="date">可空日期。</param>
		/// <returns>日期時間字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowDateTime(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowDateTime(date.Value);
		}
		/// <summary>將可空日期格式化為短時間字串。</summary>
		/// <param name="date">可空日期。</param>
		/// <returns>時間字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowTime(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowTime(date.Value);
		}





		/*#############################################################*/

		/// <summary>將 `DateTimeOffset` 格式化為短日期字串。</summary>
		/// <param name="date">日期時間。</param>
		/// <returns>短日期字串。</returns>
		public static string ShowDate(this DateTimeOffset date)
		{
			return ShowDate(date.DateTime);
		}
		/// <summary>將 `DateTimeOffset` 格式化為日期時間字串。</summary>
		/// <param name="date">日期時間。</param>
		/// <returns>日期時間字串。</returns>
		public static string ShowDateTime(this DateTimeOffset date)
		{
			return ShowDateTime(date.DateTime);
		}
		/// <summary>將 `DateTimeOffset` 格式化為短時間字串。</summary>
		/// <param name="date">日期時間。</param>
		/// <returns>時間字串。</returns>
		public static string ShowTime(this DateTimeOffset date)
		{
			return ShowTime(date.DateTime);
		}

		/// <summary>將可空 `DateTimeOffset` 格式化為短日期字串。</summary>
		/// <param name="date">可空日期時間。</param>
		/// <returns>短日期字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowDate(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowDate(date.Value.DateTime);
		}
		/// <summary>將可空 `DateTimeOffset` 格式化為日期時間字串。</summary>
		/// <param name="date">可空日期時間。</param>
		/// <returns>日期時間字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowDateTime(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowDateTime(date.Value.DateTime);
		}
		/// <summary>將可空 `DateTimeOffset` 格式化為短時間字串。</summary>
		/// <param name="date">可空日期時間。</param>
		/// <returns>時間字串；為 `null` 時回傳 `null`。</returns>
		public static string ShowTime(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowTime(date.Value.DateTime);
		}





		/// <summary>將 `DateTimeOffset` 依指定時區基準修補偏移量。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="zone">時區資訊。</param>
		/// <returns>修補後的日期時間偏移值。</returns>
		public static DateTimeOffset PatchZone(this DateTimeOffset value, TimeZoneInfo zone)
		{
			if (zone == null || value.Offset == zone.BaseUtcOffset) { return value; }

			TimeSpan diff = value.Offset - zone.BaseUtcOffset;
			value = TimeZoneInfo.ConvertTime(value, zone).Add(diff);
			return value;
		}


		/// <summary>將可空 `DateTimeOffset` 依指定時區基準修補偏移量。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="zone">時區資訊。</param>
		/// <returns>修補後的值；輸入為 `null` 時回傳 `null`。</returns>
		public static DateTimeOffset? PatchZone(this DateTimeOffset? value, TimeZoneInfo zone)
		{
			if (value == null) { return value; }
			return PatchZone(value.Value, zone);
		}


		/// <summary>將 `DateTimeOffset` 轉換到指定時區。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="zone">時區資訊。</param>
		/// <returns>轉換後的日期時間偏移值。</returns>
		public static DateTimeOffset ConvertZone(this DateTimeOffset value, TimeZoneInfo zone)
		{
			if (zone == null || value.Offset == zone.BaseUtcOffset) { return value; }

			value = TimeZoneInfo.ConvertTime(value, zone);
			return value;
		}


		/// <summary>將可空 `DateTimeOffset` 轉換到指定時區。</summary>
		/// <param name="value">欄位值。</param>
		/// <param name="zone">時區資訊。</param>
		/// <returns>轉換後的值；輸入為 `null` 時回傳 `null`。</returns>
		public static DateTimeOffset? ConvertZone(this DateTimeOffset? value, TimeZoneInfo zone)
		{
			if (value == null) { return value; }
			return ConvertZone(value.Value, zone);
		}


	}
}
