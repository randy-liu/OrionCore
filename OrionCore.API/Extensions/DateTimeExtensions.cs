using System;

namespace Orion.Api.Extensions
{
	/// <summary>日期時間區段 的 Extension</summary>
	public static class DateTimeExtensions
	{

		/// <summary>當月第一天</summary>
		public static DateTime FirstDateOfMonth(this DateTime date) 
		{
			return new DateTime(date.Year, date.Month, 1); 
		}

		/// <summary>當月最後一天</summary>
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

		/// <summary>顯示活動時間</summary>
		public static string ToLiveTime(this DateTime datatime)
		{
			string liveTime = getLiveTime(
				DateTime.Now - datatime,
				DateTime.Now.Year - datatime.Year
			);
			return liveTime;
		}
		/// <summary>顯示活動時間</summary>
		public static string ToLiveTime(this DateTime? datatime)
		{
			if (datatime == null) { return null; }
			return ToLiveTime(datatime.Value);
		}









		/*####################################################################*/

		/// <summary>顯示活動時間</summary>
		public static string ToLiveTime(this DateTimeOffset datatime)
		{
			string liveTime = getLiveTime(
				DateTimeOffset.Now - datatime,
				DateTimeOffset.Now.Year - datatime.Year
			);
			return liveTime;
		}

		/// <summary>顯示活動時間</summary>
		public static string ToLiveTime(this DateTimeOffset? datatime)
		{
			if (datatime == null) { return null; }
			return ToLiveTime(datatime.Value);
		}





		/*#############################################################*/

		/// <summary></summary>
		public static string ShowDate(this DateTime date)
		{
			return date.ToString("d");
			//return date.ToString("yyyy-MM-dd");
		}
		/// <summary></summary>
		public static string ShowDateTime(this DateTime date)
		{
			return date.ToString();
			//return date.ToString("yyyy-MM-dd HH:mm:ss");
		}
		/// <summary></summary>
		public static string ShowTime(this DateTime date)
		{
			return date.ToString("t");
			//return date.ToString("HH:mm:ss");
		}

		/// <summary></summary>
		public static string ShowDate(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowDate(date.Value);
		}
		/// <summary></summary>
		public static string ShowDateTime(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowDateTime(date.Value);
		}
		/// <summary></summary>
		public static string ShowTime(this DateTime? date)
		{
			if (date == null) { return null; }
			return ShowTime(date.Value);
		}





		/*#############################################################*/

		/// <summary></summary>
		public static string ShowDate(this DateTimeOffset date)
		{
			return ShowDate(date.DateTime);
		}
		/// <summary></summary>
		public static string ShowDateTime(this DateTimeOffset date)
		{
			return ShowDateTime(date.DateTime);
		}
		/// <summary></summary>
		public static string ShowTime(this DateTimeOffset date)
		{
			return ShowTime(date.DateTime);
		}

		/// <summary></summary>
		public static string ShowDate(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowDate(date.Value.DateTime);
		}
		/// <summary></summary>
		public static string ShowDateTime(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowDateTime(date.Value.DateTime);
		}
		/// <summary></summary>
		public static string ShowTime(this DateTimeOffset? date)
		{
			if (date == null) { return null; }
			return ShowTime(date.Value.DateTime);
		}





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
