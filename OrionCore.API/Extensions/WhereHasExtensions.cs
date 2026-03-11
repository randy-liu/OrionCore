using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Orion.Api.Extensions
{
    /// <summary>定義 IEnumerable&lt;TSource&gt; 的 Extension</summary>
    public static class WhereHasExtensions
    {

        private static bool hasQueryValue(LambdaExpression lambdaExpr)
        {
            var findList = lambdaExpr.FindByType<MemberExpression>();

            var memberExpr = findList.FirstOrDefault(x => x.ToString().StartsWith("value("));
            if (memberExpr == null) { return true; } /* 找不到 memberExpr 一律當有效條件 */

            var value = Expression.Lambda(memberExpr).Compile().DynamicInvoke();
            return OrionUtils.HasValue(value);
        }





        /// <summary>若條件運算式中的外部值有效，才套用 `Where` 篩選。</summary>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>條件有效時回傳篩選結果，否則回傳原序列。</returns>
        public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate.Compile());
        }

        /// <summary>若條件運算式中的外部值有效，才套用含索引的 `Where` 篩選。</summary>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>條件有效時回傳篩選結果，否則回傳原序列。</returns>
        public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate.Compile());
        }



        /// <summary>若條件運算式中的外部值有效，才套用查詢式 `Where` 篩選。</summary>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>條件有效時回傳篩選查詢，否則回傳原查詢。</returns>
        public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate);
        }

        /// <summary>若條件運算式中的外部值有效，才套用含索引的查詢式 `Where` 篩選。</summary>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <param name="source">來源資料。</param>
        /// <param name="predicate">篩選條件。</param>
        /// <returns>條件有效時回傳篩選查詢，否則回傳原查詢。</returns>
        public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate);
        }


    }
}
