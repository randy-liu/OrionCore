using System;

namespace Orion.Api.Models
{
	/// <summary>日期時間區段</summary>
	public class DateTimeSection : IEquatable<DateTimeSection>
	{
		/// <summary>開始日期時間</summary>
		public DateTimeOffset Start { get; set; }

		/// <summary>結束日期時間</summary>
		public DateTimeOffset End { get; set; }

		/// <summary>日期時間長度</summary>
		public TimeSpan Duration { get { return End - Start; } }


		/// <summary>建立空白日期區段，起訖時間皆為預設值。</summary>
		public DateTimeSection() { }

		/// <summary>以既有日期區段的起訖時間建立新實例。</summary>
		/// <param name="from">要複製起訖時間的來源區段。</param>
		public DateTimeSection(DateTimeSection from)
		{
			Start = from.Start;
			End = from.End;
		}

		/// <summary>以指定開始與結束時間建立日期區段。</summary>
		/// <param name="start">區段開始時間。</param>
		/// <param name="end">區段結束時間。</param>
		public DateTimeSection(DateTimeOffset start, DateTimeOffset end)
		{
			Start = start;
			End = end;
		}

		/// <summary>將字串解析為時間後建立日期區段。</summary>
		/// <param name="start">可由 <see cref="DateTimeOffset.Parse(string)"/> 解析的開始時間字串。</param>
		/// <param name="end">可由 <see cref="DateTimeOffset.Parse(string)"/> 解析的結束時間字串。</param>
		public DateTimeSection(string start, string end)
		{
			Start = DateTimeOffset.Parse(start);
			End = DateTimeOffset.Parse(end);
		}


		/// <summary>判斷指定時間是否在區段內（含邊界）。</summary>
		/// <param name="item">要判斷的時間。</param>
		/// <returns>在區段內時回傳 `true`。</returns>
		public bool Contains(DateTimeOffset item) 
		{
			return Start <= item && item <= End;
		}



		/// <summary>判斷目前區段是否與指定物件代表相同起訖時間。</summary>
		/// <param name="other">要比較的物件。</param>
		/// <returns>當 <paramref name="other"/> 可轉為 <see cref="DateTimeSection"/> 且起訖時間相同時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
		public override bool Equals(object other)
		{
			return Equals(other as DateTimeSection);
		}
		/// <summary>判斷目前區段是否與另一日期區段相等。</summary>
		/// <param name="other">要比較的日期區段。</param>
		/// <returns>當兩者皆非 <c>null</c> 且起訖時間完全相同時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
		public bool Equals(DateTimeSection other)
		{
			if (ReferenceEquals(other, null)) { return false; }
			if (ReferenceEquals(other, this)) { return true; }
			return Start == other.Start && End == other.End;
		}



		/// <summary>取得日期區段雜湊碼。</summary>
		/// <returns>雜湊碼。</returns>
		public override int GetHashCode()
		{
			return Start.GetHashCode() ^ End.GetHashCode();
		}

		/// <summary>轉為可讀字串格式。</summary>
		/// <returns>日期區段字串。</returns>
		public override string ToString()
		{
			return $"{Start:yyyy-MM-dd HH:mm:ss.fff} ~ {End:yyyy-MM-dd HH:mm:ss.fff}";
		}



		/// <summary>比較兩個日期區段是否相等。</summary>
		/// <param name="a">左側日期區段。</param>
		/// <param name="b">右側日期區段。</param>
		/// <returns>兩者皆為 <c>null</c> 或起訖時間相同時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
		public static bool operator ==(DateTimeSection a, DateTimeSection b)
		{
			if (ReferenceEquals(a, null))
			{
				/* null == null = true. */
				if (ReferenceEquals(b, null)) { return true; }

				/* Only the left side is null.*/
				return false;
			}
			return a.Equals(b);
		}

		/// <summary>比較兩個日期區段是否不相等。</summary>
		/// <param name="a">左側日期區段。</param>
		/// <param name="b">右側日期區段。</param>
		/// <returns>當兩者不符合相等條件時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
		public static bool operator !=(DateTimeSection a, DateTimeSection b)
		{
			return !(a == b);
		}
	}
}
