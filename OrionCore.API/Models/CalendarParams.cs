using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.Api.Models
{
    /// <summary>提供單月日曆顯示與日期判斷所需的參數與工具方法。</summary>
    public class CalendarParams
    {
        private string[] _weekName = new[]
        {
            "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"
        };


        private int _year = DateTime.Today.Year;
        private int _month = DateTime.Today.Month;

        private DateTime _firstDate;
        private DateTime _lastDate;

        private DayOfWeek _weekDayFirst = DayOfWeek.Sunday;
        private DayOfWeek _weekDayLast = DayOfWeek.Saturday;


        /// <summary>建立日曆參數並初始化當月日期區間。</summary>
        public CalendarParams()
        {
            updateStartEnd();
        }


        private void updateStartEnd()
        {
            _firstDate = new DateTime(_year, _month, 1);
            _lastDate = _firstDate.AddMonths(1).AddDays(-1);
        }


        /// <summary>目前日曆年份；設定後會同步更新當月首尾日期。</summary>
        public int Year
        {
            get { return _year; }
            set { _year = value; updateStartEnd(); }
        }

        /// <summary>目前日曆月份；設定後會同步更新當月首尾日期。</summary>
        public int Month
        {
            get { return _month; }
            set { _month = value; updateStartEnd(); }
        }



        /// <summary>每週第一天；設定後會自動調整對應的每週最後一天。</summary>
        public DayOfWeek WeekDayFirst
        {
            get { return _weekDayFirst; }
            set
            {
                _weekDayFirst = value;
                _weekDayLast = (DayOfWeek)((int)(value + 6) % 7);
            }
        }

        /// <summary>每週最後一天（由 <see cref="WeekDayFirst"/> 推算）。</summary>
        public DayOfWeek WeekDayLast { get { return _weekDayLast; } }

        /// <summary>當月第一天日期。</summary>
        public DateTime FirstDate { get { return _firstDate; } }

        /// <summary>當月最後一天日期。</summary>
        public DateTime LastDate { get { return _lastDate; } }


        /// <summary>補齊到完整週顯示時的第一天日期。</summary>
        public DateTime PadFirstDate
        {
            get
            {
                DateTime firstPad = _firstDate;
                while (firstPad.DayOfWeek != _weekDayFirst) { firstPad = firstPad.AddDays(-1); }
                return firstPad;
            }
        }

        /// <summary>補齊到完整週顯示時的最後一天日期。</summary>
        public DateTime PadLastDate
        {
            get
            {
                DateTime lastPad = _lastDate;
                while (lastPad.DayOfWeek != _weekDayLast) { lastPad = lastPad.AddDays(1); }
                return lastPad;
            }
        }

        /// <summary>以上月同日（當月第一天往前一個月）表示的日期。</summary>
        public DateTime PreviousMonth { get { return _firstDate.AddMonths(-1); } }

        /// <summary>以下月同日（當月第一天往後一個月）表示的日期。</summary>
        public DateTime NextMonth { get { return _firstDate.AddMonths(1); } }


        /// <summary>依每週起始日順序回傳星期名稱清單。</summary>
        public string[] WeekNameItems
        {
            get
            {
                int weekDay = (int)_weekDayFirst;
                return _weekName.Skip(weekDay).Concat(_weekName.Take(weekDay)).ToArray();
            }
        }


        /// <summary>取得以目前年份為中心的連續年份清單。</summary>
        /// <param name="count">要回傳的年份總數。</param>
        /// <returns>從目前年份往前 <c>count / 2</c> 年開始的連續年份陣列。</returns>
        public int[] GetYearItems(int count = 20)
        {
            return Enumerable.Range(_year - (count / 2), count).ToArray();
        }

        /// <summary>取得 1 到 12 月的月份清單。</summary>
        /// <returns>包含 1~12 的月份陣列。</returns>
        public int[] GetMonthItems()
        {
            return Enumerable.Range(1, 12).ToArray();
        }

        /// <summary>判斷指定日期是否為系統今天日期。</summary>
        /// <param name="date">要判斷的日期。</param>
        /// <returns>日期等於 <see cref="DateTime.Today"/> 時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
        public bool IsToday(DateTime date)
        {
            return date == DateTime.Today;
        }

        /// <summary>判斷指定日期是否為週末（星期六或星期日）。</summary>
        /// <param name="date">要判斷的日期。</param>
        /// <returns>為星期六或星期日時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
        public bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>判斷指定日期是否落在目前月份區間（含首尾日）。</summary>
        /// <param name="date">要判斷的日期。</param>
        /// <returns>日期介於當月第一天與最後一天之間（含邊界）時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
        public bool IsInMonth(DateTime date)
        {
            return _firstDate <= date && date <= _lastDate;
        }


        /// <summary>判斷指定日期是否為設定的每週第一天。</summary>
        /// <param name="date">要判斷的日期。</param>
        /// <returns>星期值等於 <see cref="WeekDayFirst"/> 時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
        public bool IsWeekDayFirst(DateTime date)
        {
            return WeekDayFirst == date.DayOfWeek;
        }

        /// <summary>判斷指定日期是否為設定的每週最後一天。</summary>
        /// <param name="date">要判斷的日期。</param>
        /// <returns>星期值等於 <see cref="WeekDayLast"/> 時回傳 <c>true</c>，否則回傳 <c>false</c>。</returns>
        public bool IsWeekDayLast(DateTime date)
        {
            return WeekDayLast == date.DayOfWeek;
        }


        /// <summary>依日期遞增列舉目前月份的每一天。</summary>
        /// <returns>從 <see cref="FirstDate"/> 到 <see cref="LastDate"/>（含邊界）的日期序列。</returns>
        public IEnumerable<DateTime> EnumerateDates()
        {
            for (var date = FirstDate; date <= LastDate; date = date.AddDays(1))
            {
                yield return date;
            }
        }


        /// <summary>依日期遞增列舉補齊完整週後的每一天。</summary>
        /// <returns>從 <see cref="PadFirstDate"/> 到 <see cref="PadLastDate"/>（含邊界）的日期序列。</returns>
        public IEnumerable<DateTime> EnumeratePadDates()
        {
            for (var date = PadFirstDate; date <= PadLastDate; date = date.AddDays(1))
            {
                yield return date;
            }
        }

    }
}

