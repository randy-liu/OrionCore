using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace OrionCore.API.Extensions
{
	/// <summary>定義 IEnumerable 與泛型 IEnumerable 的 Extension</summary>
	public static class EnumerableExtensions
	{


		/// <summary>附加 Item 到 IEnumerable 的最後面</summary>
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, params TSource[] items)
		{
			return source.Concat(items);
		}

		/// <summary>從 IEnumerable 建立 List</summary>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToList();
		}
		/// <summary>從 IEnumerable 建立 List</summary>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			return source.Select(selector).ToList();
		}

		/// <summary>從 IEnumerable 建立 List</summary>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}
		/// <summary>從 IEnumerable 建立 List</summary>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}
        

        /// <summary>source 如果為 null 就回傳 Empty</summary>
        public static IEnumerable<T> NullToEmpty<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }


        /// <summary>加入項目到 IList</summary>
        public static void AddRangeTo(this IEnumerable source, IList collection)
		{
			foreach (var item in source) { collection.Add(item); }
		}

		/// <summary>加入項目到 ICollection&lt;T&gt;</summary>
		public static void AddRangeTo<T>(this IEnumerable<T> source, ICollection<T> collection)
		{
			foreach (var item in source) { collection.Add(item); }
		}



		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration) 
			{ 
				action(item);
			}
		}

		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;
			foreach (T item in enumeration)
			{
				action(item, i++);
			}
		}



		/// <summary> 賦予 IEnumerable Each Method</summary>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
				yield return item;
			}
		}

		/// <summary> 賦予 IEnumerable Each Method</summary>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;
			foreach (T item in enumeration)
			{
				action(item, i++);
				yield return item;
			}
		}


		/// <summary> 到 string.Join, 輸入 null 則回傳 null</summary>
		public static string JoinBy<TSource>(this IEnumerable<TSource> enumeration, string separator)
		{
			if(enumeration == null) { return null; }
			return string.Join(separator, enumeration);
		}
         




        /// <summary>從 IEnumerable 建立 HashSet</summary>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumeration)
		{
			return new HashSet<T>(enumeration);
		}

		/// <summary>從 IEnumerable 建立 HashSet</summary>
		public static HashSet<T> ToHashSet<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
		{
			return source.Select(selector).ToHashSet();
		}
		/// <summary>從 IEnumerable 建立 HashSet</summary>
		public static HashSet<T> ToHashSet<TSource, T>(this IEnumerable<TSource> source, Func<TSource, int, T> selector)
		{
			return source.Select(selector).ToHashSet();
		}



		/// <summary> OrderBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		/// <typeparam name="TSource">來源型態</typeparam>
		/// <typeparam name="TKey">選擇欄位型態</typeparam>
		/// <param name="source">Extension Method this</param>
		/// <param name="keySelector">選擇排序欄位的 Delegate</param>
		/// <param name="descending">正反排序</param>
		/// <returns>IOrderedEnumerable&lt;TSource&gt;</returns>
		/// <example>
		///	<code>
		///		var list = new List&lt;int&gt;(){2,5,1,3,7,4};
		///		IEnumerableExtensions.OrderBy&lt;int, int&gt;(list, x =&gt; x, true);
		///		list.OrderBy&lt;int, int&gt;(x =&gt; x, true);
		/// </code>
		/// </example>
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
		{
			return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
		}


		/// <summary> ThenBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
		{
			return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
		}

		 



		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MaxOrDefault(x => x);
		}

		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MaxOrDefault(selector);
		}


		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MinOrDefault(x => x);
		}

		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MinOrDefault(selector);
		}








		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault(this IEnumerable<int> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault(this IEnumerable<long> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault(this IEnumerable<float> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault(this IEnumerable<double> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault(this IEnumerable<decimal> source) { return source.SumOrDefault(x => x); }



		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static int SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static long SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static float SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static double SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}





		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault(this IEnumerable<int> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault(this IEnumerable<long> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static float AverageOrDefault(this IEnumerable<float> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault(this IEnumerable<double> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static decimal AverageOrDefault(this IEnumerable<decimal> source)
		{
			return source.DefaultIfEmpty().Average();
		}

		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static float AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		public static decimal AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}





		/// <summary>根據泛型參數轉型，若失敗則拋棄</summary>
		public static IEnumerable<TResult> Convert<TResult>(this IEnumerable source)
		{
			return Convert(source, typeof(TResult)).Cast<TResult>();
		}



		/// <summary>根據 type 參數轉型，若失敗則拋棄</summary>
		public static IEnumerable<object> Convert(this IEnumerable source, Type type)
		{
			foreach (var value in source)
			{
				object result = OrionUtils.ConvertType(value, type);
				if (result == null) { continue; }

				yield return result;
			}
		}




		/// <summary>將資料依數量分區段</summary>
		public static IEnumerable<IEnumerable<T>> Bulk<T>(this IEnumerable<T> source, int blockSize)
		{
			if (source == null || !source.Any()) { return Enumerable.Empty<IEnumerable<T>>(); }

			return source
				.Select((value, i) => new { Value = value, Tag = i / blockSize })
				.GroupBy(x => x.Tag)
				.Select(g => g.Select(x => x.Value));
		}


		/// <summary>將資料依數量分區段</summary>
		public static IEnumerable<List<T>> BulkToList<T>(this IEnumerable<T> source, int blockSize)
		{
			return Bulk(source, blockSize).Select(x => x.ToList());
		}



		/// <summary>尋訪所有節點，廣度優先</summary>
		public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
		{
			var queue = new Queue<T>(items);
			while (queue.Count > 0)
			{
				T item = queue.Dequeue();
				yield return item;

				IEnumerable<T> childs = childSelector(item);
				if(childs == null) { continue; }
				foreach (var child in childs) { queue.Enqueue(child); }
			}
		}



		/// <summary>準備資料 List</summary>
		public static Func<TKey, List<TSource>> PrepareList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return PrepareList(source, keySelector, x => x);
		}
		/// <summary>準備資料 List</summary>
		public static Func<TKey, List<TValue>> PrepareList<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
		{
			Dictionary<TKey, List<TValue>> data = source
				.GroupBy(keySelector)
				.ToDictionary(
					g => g.Key,
					g => g.Select(valueSelector).ToList()
				);

			return key => data.ContainsKey(key) ? data[key] : new List<TValue>();
		}



	}
}
