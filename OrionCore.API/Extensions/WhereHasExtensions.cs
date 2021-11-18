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





        /// <summary>判斷是否有值後進行 where 篩選</summary>
        public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate.Compile());
        }

        /// <summary>判斷是否有值後進行 where 篩選</summary>
        public static IEnumerable<TSource> WhereHas<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate.Compile());
        }



        /// <summary>判斷是否有值後進行 where 篩選</summary>
        public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate);
        }

        /// <summary>判斷是否有值後進行 where 篩選</summary>
        public static IQueryable<TSource> WhereHas<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (!hasQueryValue(predicate)) { return source; }
            return source.Where(predicate);
        }


    }
}
