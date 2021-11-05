using System;

namespace OrionCore.API.Models
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


		/// <summary></summary>
		public DateTimeSection() { }

		/// <summary></summary>
		public DateTimeSection(DateTimeSection from)
		{
			Start = from.Start;
			End = from.End;
		}

		/// <summary></summary>
		public DateTimeSection(DateTimeOffset start, DateTimeOffset end)
		{
			Start = start;
			End = end;
		}

		/// <summary></summary>
		public DateTimeSection(string start, string end)
		{
			Start = DateTimeOffset.Parse(start);
			End = DateTimeOffset.Parse(end);
		}


		/// <summary></summary>
		public override bool Equals(object other)
		{
			return Equals(other as DateTimeSection);
		}
		/// <summary></summary>
		public bool Equals(DateTimeSection other)
		{
			if (ReferenceEquals(other, null)) { return false; }
			if (ReferenceEquals(other, this)) { return true; }
			return Start == other.Start && End == other.End;
		}



		/// <summary></summary>
		public override int GetHashCode()
		{
			return Start.GetHashCode() ^ End.GetHashCode();
		}

		/// <summary></summary>
		public override string ToString()
		{
			return $"{Start:yyyy-MM-dd HH:mm:ss.fff} ~ {End:yyyy-MM-dd HH:mm:ss.fff}";
		}


		/// <summary>覆寫等於運算子</summary>
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

		/// <summary>覆寫不等於運算子</summary>
		public static bool operator !=(DateTimeSection a, DateTimeSection b)
		{
			return !(a == b);
		}
	}
}
