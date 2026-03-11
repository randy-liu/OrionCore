using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Orion.Api.Models;


namespace Orion.Api.Extensions
{
    /// <summary>定義 IQueryable&lt;TSource&gt; 的 Extension</summary>
    public static class QueryableExtensions
    {

        /// <summary>將查詢結果投影後轉為 `List`。</summary>
        /// <param name="source">要投影的來源查詢。</param>
        /// <param name="selector">將來源元素轉為目標型別的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TResult">投影結果型別。</typeparam>
        /// <returns>投影後的清單。</returns>
        public static List<TResult> ToList<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToList();
        }

        /// <summary>將查詢結果投影後轉為陣列。</summary>
        /// <param name="source">要投影的來源查詢。</param>
        /// <param name="selector">將來源元素轉為目標型別的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TResult">投影結果型別。</typeparam>
        /// <returns>投影後的陣列。</returns>
        public static TResult[] ToArray<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToArray();
        }




        /*====================================================*/


        /// <summary>依排序鍵與方向建立第一層排序。</summary>
        /// <param name="source">要排序的來源查詢。</param>
        /// <param name="keySelector">用來取得排序鍵值的運算式。</param>
        /// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TKey">排序鍵值型別。</typeparam>
        /// <returns>已排序查詢。</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool descending)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        /// <summary>在既有排序後追加次排序條件。</summary>
        /// <param name="source">前一次排序後的來源查詢。</param>
        /// <param name="keySelector">用來取得次排序鍵值的運算式。</param>
        /// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TKey">排序鍵值型別。</typeparam>
        /// <returns>追加次排序後的查詢。</returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool descending)
        {
            return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
        }


        /// <summary>以列舉名稱對應屬性名稱進行排序。</summary>
        /// <param name="source">要排序的來源查詢。</param>
        /// <param name="keySelector">代表排序欄位名稱的列舉值。</param>
        /// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>已排序查詢。</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, Enum keySelector, bool descending)
        {
            return OrderBy(source, keySelector.ToString(), descending);
        }

        /// <summary>依屬性名稱動態建立排序。</summary>
        /// <param name="source">要排序的來源查詢。</param>
        /// <param name="keySelector">屬性名稱，用於動態指定排序欄位。</param>
        /// <param name="descending">`true` 表示遞減（DESC）；`false` 表示遞增（ASC）。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>已排序查詢。</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string keySelector, bool descending)
        {
            Type modelType = typeof(TSource);
            var prop = modelType.GetProperty(keySelector);
            if (prop == null) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }

            MethodInfo orderByMethod = Utils.QueryableOrderByMethod(modelType, prop.PropertyType);
            var keyExpression = Utils.KeyExpression(modelType, prop);

            return (IOrderedQueryable<TSource>)orderByMethod.Invoke(null, new object[] { source, keyExpression, descending });
        }




        /// <summary>依逗號分隔欄位字串建立多欄位排序。</summary>
        /// <param name="source">要排序的來源查詢。</param>
        /// <param name="keySelector">以逗號分隔的排序欄位字串；欄位前加 `-` 表示遞減排序。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>多欄位排序後的查詢。</returns>
        public static IOrderedQueryable<TSource> AdvancedOrderBy<TSource>(this IQueryable<TSource> source, string keySelector)
        {
            Type modelType = typeof(TSource);

            var columns = keySelector.Split(',')
                .Select(x => x.Trim())
                .Where(x => x.HasText())
                .Select(x => new
                {
                    Prop = modelType.GetProperty(x.Trim('-')),
                    Descending = x.StartsWith("-"),
                })
                .Where(x => x.Prop != null)
                .Distinct(x => x.Prop.Name)
                .ToList();

            if (columns.Count == 0) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }


            var first = columns.Shift();
            IOrderedQueryable<TSource> ordered = OrderBy(source, first.Prop.Name, first.Descending);


            foreach (var column in columns)
            {
                Type propType = column.Prop.PropertyType;
                MethodInfo thenByMethod = Utils.QueryableThenByMethod(modelType, propType);
                var keyExpression = Utils.KeyExpression(modelType, column.Prop);

                ordered = (IOrderedQueryable<TSource>)thenByMethod.Invoke(null, new object[] { ordered, keyExpression, column.Descending });
            }

            return ordered;
        }





        /*====================================================*/

        /// <summary>避免 SqlParameter 參數數量超過 2100 個，將查詢 IN 分次執行。</summary>
        /// <param name="source">要套用篩選條件的來源查詢。</param>
        /// <param name="selector">指定要比對欄位的運算式。</param>
        /// <param name="values">`IN` 條件使用的比對值集合。</param>
        /// <param name="blockSize">每次切分的值數量，用於避免單次參數過多。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="T">比對值型別。</typeparam>
        /// <returns>符合 IN 條件的結果序列。</returns>
        public static IEnumerable<TSource> WhereIn<TSource, T>(this IQueryable<TSource> source, Expression<Func<TSource, T>> selector, IEnumerable<T> values, int blockSize = 2000)
        {
            if (values == null || !values.Any()) { return Enumerable.Empty<TSource>(); }

            MethodInfo containsMethod = LambdaUtils.GetMethod(() => values.Contains(default(T)));

            IEnumerable<TSource> result = values
                .Distinct()
                .BulkToList(blockSize)
                .SelectMany(valueList =>
                {
                    var containsExpr = Expression.Call(containsMethod, Expression.Constant(valueList), selector.Body);
                    return source.Where(Expression.Lambda<Func<TSource, bool>>(containsExpr, selector.Parameters));
                });

            return result;
        }





        /*====================================================*/


        private static Expression<Func<TSource, TResult?>> convertToNullable<TSource, TResult>(Expression<Func<TSource, TResult>> selector) where TResult : struct
        {
            Type nType = typeof(Nullable<>).MakeGenericType(typeof(TResult));
            var convertExpr = Expression.Convert(selector.Body, nType);
            var selectorExpr = Expression.Lambda<Func<TSource, TResult?>>(convertExpr, selector.Parameters);

            return selectorExpr;
        }



        /// <summary>傳回最大的結果值；空集合則傳回預設值。</summary>
        /// <param name="source">要取得最大值的來源查詢。</param>
        /// <typeparam name="TSource">元素型別（實值型別）。</typeparam>
        /// <returns>最大值；空集合時回傳該型別預設值。</returns>
        public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source) where TSource : struct
        {
            return source.MaxOrDefault(x => x);
        }

        /// <summary>傳回最大的結果值；空集合則傳回預設值。</summary>
        /// <param name="source">要取得最大值的來源查詢。</param>
        /// <param name="selector">將元素投影為可比較值的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TResult">比較值型別（實值型別）。</typeparam>
        /// <returns>最大值；空集合時回傳該型別預設值。</returns>
        public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
        {
            return source.Max(convertToNullable(selector)) ?? default(TResult);
        }



        /// <summary>傳回最小的結果值；空集合則傳回預設值。</summary>
        /// <param name="source">要取得最小值的來源查詢。</param>
        /// <typeparam name="TSource">元素型別（實值型別）。</typeparam>
        /// <returns>最小值；空集合時回傳該型別預設值。</returns>
        public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source) where TSource : struct
        {
            return source.MinOrDefault(x => x);
        }

        /// <summary>傳回最小的結果值；空集合則傳回預設值。</summary>
        /// <param name="source">要取得最小值的來源查詢。</param>
        /// <param name="selector">將元素投影為可比較值的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TResult">比較值型別（實值型別）。</typeparam>
        /// <returns>最小值；空集合時回傳該型別預設值。</returns>
        public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
        {
            return source.Min(convertToNullable(selector)) ?? default(TResult);
        }



        /*====================================================*/

        /// <summary>計算 `int` 查詢的總和；空集合時回傳 0。</summary>
        /// <returns>`int` 總和。</returns>
        public static int SumOrDefault(this IQueryable<int> source) { return source.SumOrDefault(x => x); }

        /// <summary>計算 `long` 查詢的總和；空集合時回傳 0。</summary>
        /// <returns>`long` 總和。</returns>
        public static long SumOrDefault(this IQueryable<long> source) { return source.SumOrDefault(x => x); }

        /// <summary>計算 `float` 查詢的總和；空集合時回傳 0。</summary>
        /// <returns>`float` 總和。</returns>
        public static float SumOrDefault(this IQueryable<float> source) { return source.SumOrDefault(x => x); }

        /// <summary>計算 `double` 查詢的總和；空集合時回傳 0。</summary>
        /// <returns>`double` 總和。</returns>
        public static double SumOrDefault(this IQueryable<double> source) { return source.SumOrDefault(x => x); }

        /// <summary>計算 `decimal` 查詢的總和；空集合時回傳 0。</summary>
        /// <returns>`decimal` 總和。</returns>
        public static decimal SumOrDefault(this IQueryable<decimal> source) { return source.SumOrDefault(x => x); }



        /// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
        /// <param name="source">要計算總和的來源查詢。</param>
        /// <param name="selector">將元素投影為 `int` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>總和值；空集合時回傳 0。</returns>
        public static int SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return source.Sum(convertToNullable(selector)) ?? default(int);
        }
        /// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
        /// <param name="source">要計算總和的來源查詢。</param>
        /// <param name="selector">將元素投影為 `long` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>總和值；空集合時回傳 0。</returns>
        public static long SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return source.Sum(convertToNullable(selector)) ?? default(long);
        }
        /// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
        /// <param name="source">要計算總和的來源查詢。</param>
        /// <param name="selector">將元素投影為 `float` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>總和值；空集合時回傳 0。</returns>
        public static float SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return source.Sum(convertToNullable(selector)) ?? default(float);
        }
        /// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
        /// <param name="source">要計算總和的來源查詢。</param>
        /// <param name="selector">將元素投影為 `double` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>總和值；空集合時回傳 0。</returns>
        public static double SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return source.Sum(convertToNullable(selector)) ?? default(double);
        }
        /// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
        /// <param name="source">要計算總和的來源查詢。</param>
        /// <param name="selector">將元素投影為 `decimal` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>總和值；空集合時回傳 0。</returns>
        public static decimal SumOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return source.Sum(convertToNullable(selector)) ?? default(decimal);
        }




        /// <summary>計算 `int` 查詢的平均值；空集合時回傳 0。</summary>
        /// <returns>`double` 平均值。</returns>
        public static double AverageOrDefault(this IQueryable<int> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算 `long` 查詢的平均值；空集合時回傳 0。</summary>
        /// <returns>`double` 平均值。</returns>
        public static double AverageOrDefault(this IQueryable<long> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算 `float` 查詢的平均值；空集合時回傳 0。</summary>
        /// <returns>`float` 平均值。</returns>
        public static float AverageOrDefault(this IQueryable<float> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算 `double` 查詢的平均值；空集合時回傳 0。</summary>
        /// <returns>`double` 平均值。</returns>
        public static double AverageOrDefault(this IQueryable<double> source) { return source.AverageOrDefault(x => x); }
        /// <summary>計算 `decimal` 查詢的平均值；空集合時回傳 0。</summary>
        /// <returns>`decimal` 平均值。</returns>
        public static decimal AverageOrDefault(this IQueryable<decimal> source) { return source.AverageOrDefault(x => x); }



        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        /// <param name="source">要計算平均值的來源查詢。</param>
        /// <param name="selector">將元素投影為 `int` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>平均值；空集合時回傳 0。</returns>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(int);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        /// <param name="source">要計算平均值的來源查詢。</param>
        /// <param name="selector">將元素投影為 `long` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>平均值；空集合時回傳 0。</returns>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(long);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        /// <param name="source">要計算平均值的來源查詢。</param>
        /// <param name="selector">將元素投影為 `float` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>平均值；空集合時回傳 0。</returns>
        public static float AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(float);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        /// <param name="source">要計算平均值的來源查詢。</param>
        /// <param name="selector">將元素投影為 `double` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>平均值；空集合時回傳 0。</returns>
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(double);
        }
        /// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
        /// <param name="source">要計算平均值的來源查詢。</param>
        /// <param name="selector">將元素投影為 `decimal` 的運算式。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <returns>平均值；空集合時回傳 0。</returns>
        public static decimal AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            return source.Average(convertToNullable(selector)) ?? default(decimal);
        }






        /*====================================================*/

        /// <summary>建立 `WhereQueryableBuilder` 以鏈式方式組合動態篩選條件。</summary>
        /// <param name="source">要套用條件建構器的來源查詢。</param>
        /// <param name="param">封裝查詢條件的 `WhereParams` 物件。</param>
        /// <typeparam name="TSource">來源元素型別。</typeparam>
        /// <typeparam name="TParams">條件參數型別。</typeparam>
        /// <returns>可持續設定條件的查詢建構器。</returns>
        public static WhereQueryableBuilder<TSource, TParams> WhereBuilder<TSource, TParams>(this IQueryable<TSource> source, WhereParams<TParams> param)
        {
            return new WhereQueryableBuilder<TSource, TParams>(source, param);
        }


    }
}
