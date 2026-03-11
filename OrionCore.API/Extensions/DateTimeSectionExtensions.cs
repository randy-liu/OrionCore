using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Orion.Api.Models;

namespace Orion.Api.Extensions
{
    /// <summary>日期時間區段 的 Extension</summary>
    public static class DateTimeSectionExtensions
    {

        /// <summary>計算兩個日期時間區段的重疊區間。</summary>
        /// <param name="sectionA">第一個要比較的日期時間區段。</param>
        /// <param name="sectionB">要比較的另一個日期時間區段。</param>
        /// <returns>由最大起點與最小終點組成的重疊區段。</returns>
        public static DateTimeSection Overlap(this DateTimeSection sectionA, DateTimeSection sectionB)
        {
            return new DateTimeSection
            {
                Start = OrionUtils.Max(sectionA.Start, sectionB.Start),
                End = OrionUtils.Min(sectionA.End, sectionB.End),
            };
        }



        /// <summary>將已占用區段轉為其間的空缺區段序列。</summary>
        /// <param name="source">已占用的日期時間區段集合。</param>
        /// <returns>由 `DateTimeOffset.MinValue` 到 `MaxValue` 的空缺區段序列。</returns>
        public static IEnumerable<DateTimeSection> InvertSection(this IEnumerable<DateTimeSection> source)
        {
            source = source.Where(x => x != null).OrderBy(x => x.Start);
            var cursor = new DateTimeSection { Start = DateTimeOffset.MinValue };

            foreach (var item in source)
            {
                cursor.End = item.Start;

                yield return cursor;
                cursor = new DateTimeSection { Start = item.End };
            }

            cursor.End = DateTimeOffset.MaxValue;
            yield return cursor;
        }



        /// <summary>計算兩組日期時間區段的交集。</summary>
        /// <param name="source">第一組要計算交集的日期時間區段集合。</param>
        /// <param name="target">第二組要計算交集的日期時間區段集合。</param>
        /// <returns>兩組區段重疊後的區段序列。</returns>
        public static IEnumerable<DateTimeSection> IntersectSection(this IEnumerable<DateTimeSection> source, IEnumerable<DateTimeSection> target)
        {
            source = source.Where(x => x != null && x.Start < x.End).OrderBy(x => x.End);
            target = target.Where(x => x != null && x.Start < x.End).OrderBy(x => x.End);

            using (IEnumerator<DateTimeSection> sIter = source.GetEnumerator(), tIter = target.GetEnumerator())
            {
                bool hasS = sIter.MoveNext();
                bool hasT = tIter.MoveNext();

                while (hasS && hasT)
                {
                    DateTimeSection overlap = Overlap(sIter.Current, tIter.Current);
                    if (overlap.End == sIter.Current.End) { hasS = sIter.MoveNext(); }
                    if (overlap.End == tIter.Current.End) { hasT = tIter.MoveNext(); }

                    if (overlap.Start < overlap.End) { yield return overlap; }
                }
            }
        }




        /// <summary>合併兩組日期時間區段並串接重疊部分。</summary>
        /// <param name="source">第一組要合併的日期時間區段集合。</param>
        /// <param name="target">第二組要合併的日期時間區段集合。</param>
        /// <returns>依開始時間排序且已合併重疊的區段序列。</returns>
        public static IEnumerable<DateTimeSection> UnionSection(this IEnumerable<DateTimeSection> source, IEnumerable<DateTimeSection> target)
        {
            source = source.Concat(target).Where(x => x != null).OrderBy(x => x.Start);

            using (IEnumerator<DateTimeSection> iter = source.GetEnumerator())
            {
                if (!iter.MoveNext()) { yield break; }
                var cursor = new DateTimeSection(iter.Current);

                while (iter.MoveNext())
                {
                    if (iter.Current.Start <= cursor.End)
                    {
                        cursor.End = OrionUtils.Max(iter.Current.End, cursor.End);
                        continue;
                    }

                    yield return cursor;
                    cursor = new DateTimeSection(iter.Current);
                }

                yield return cursor;
            }
        }



        /// <summary>計算第一組區段扣除第二組區段後的差集。</summary>
        /// <param name="first">被扣除的來源日期時間區段集合。</param>
        /// <param name="second">要從 `first` 排除的日期時間區段集合。</param>
        /// <returns>扣除後剩餘的區段序列。</returns>
        public static IEnumerable<DateTimeSection> ExceptSection(this IEnumerable<DateTimeSection> first, IEnumerable<DateTimeSection> second)
        {
            return first.IntersectSection(second.InvertSection());
        }



        /// <summary>加總日期時間區段集合的持續時間。</summary>
        /// <param name="source">要加總持續時間的日期時間區段集合。</param>
        /// <returns>所有區段持續時間的總和。</returns>
        public static TimeSpan SumDuration(this IEnumerable<DateTimeSection> source)
        {
            if (!source.Any()) { return TimeSpan.Zero; }
            return source.Select(x => x.Duration).Aggregate((sum, next) => sum + next);
        }



        /// <summary>檢查日期時間區段是否重疊</summary>
        /// <param name="source">要檢查是否互相重疊的日期時間區段集合。</param>
        public static void CheckOverlap(this IEnumerable<DateTimeSection> source)
        {
            source = source.Where(x => x != null).OrderBy(x => x.Start);

            using (IEnumerator<DateTimeSection> iter = source.GetEnumerator())
            {
                if (!iter.MoveNext()) { return; }
                DateTimeSection prev = iter.Current;

                while (iter.MoveNext())
                {
                    if (prev.End <= iter.Current.Start) { continue; }
                    throw new DataException($"Section overlap {prev} and {iter.Current}");
                }
            }
        }



        /// <summary>將同一集合中互相重疊的區段合併為不重疊序列。</summary>
        /// <param name="source">要合併重疊區段的日期時間區段集合。</param>
        /// <returns>合併後的區段序列。</returns>
        public static IEnumerable<DateTimeSection> JoinOverlap(this IEnumerable<DateTimeSection> source)
        {
            source = source.Where(x => x != null).OrderBy(x => x.Start).ThenByDescending(x => x.End);

            using (IEnumerator<DateTimeSection> iter = source.GetEnumerator())
            {
                if (!iter.MoveNext()) { yield break; }
                var item = new DateTimeSection(iter.Current);

                while (iter.MoveNext())
                {
                    if (item.End >= iter.Current.Start)
                    {
                        item.End = OrionUtils.Max(item.End, iter.Current.End);
                    }
                    else
                    {
                        yield return item;
                        item = new DateTimeSection(iter.Current);
                    }
                }
                yield return item;
            }
        }






    }
}
