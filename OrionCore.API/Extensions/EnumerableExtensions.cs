using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Orion.Api.Extensions
{
	/// <summary>定義 IEnumerable 與泛型 IEnumerable 的 Extension</summary>
	public static class EnumerableExtensions
	{

		/// <summary>以 tuple 鍵值選擇器將序列轉為字典。</summary>
		/// <param name="source">要轉換為 Dictionary 的來源序列。</param>
		/// <param name="keyValueSelector">將元素轉為 `(Key, Value)` 的對應函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TKey">鍵值型別。</typeparam>
		/// <typeparam name="TElement">值型別。</typeparam>
		/// <returns>轉換後的字典。</returns>
		public static Dictionary<TKey, TElement> ToDict<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, (TKey, TElement)> keyValueSelector)
		{
			return source.Select(keyValueSelector).ToDictionary(x => x.Item1, x => x.Item2);
		}

		/// <summary>以含索引 tuple 鍵值選擇器將序列轉為 Dictionary。</summary>
		/// <param name="source">要轉換為 Dictionary 的來源序列。</param>
		/// <param name="keyValueSelector">將元素與索引轉為 `(Key, Value)` 的對應函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TKey">鍵值型別。</typeparam>
		/// <typeparam name="TElement">值型別。</typeparam>
		/// <returns>轉換後的字典。</returns>
		public static Dictionary<TKey, TElement> ToDict<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, int, (TKey, TElement)> keyValueSelector)
		{
			return source.Select(keyValueSelector).ToDictionary(x => x.Item1, x => x.Item2);
		}



		/// <summary>在序列尾端附加一個或多個元素。</summary>
		/// <param name="source">原始序列。</param>
		/// <param name="items">要附加在序列尾端的元素。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>附加後的序列。</returns>
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, params TSource[] items)
		{
			return source.Concat(items);
		}

		/// <summary>將序列投影後轉為清單。</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素轉為目標型別的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後清單。</returns>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToList();
		}
		/// <summary>將序列（含索引）投影後轉為清單。</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素與索引轉為目標型別的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後清單。</returns>
		public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			return source.Select(selector).ToList();
		}

		/// <summary>將序列投影後轉為陣列。</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素轉為目標型別的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後陣列。</returns>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}
		/// <summary>將序列（含索引）投影後轉為陣列。</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素與索引轉為目標型別的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後陣列。</returns>
		public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			return source.Select(selector).ToArray();
		}



		/// <summary>從 IEnumerable 建立 Queue</summary>
		/// <param name="source">要轉換為 Queue 的來源序列。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>包含來源元素的佇列。</returns>
		public static Queue<TSource> ToQueue<TSource>(this IEnumerable<TSource> source)
		{
			return new Queue<TSource>(source);
		}
		/// <summary>從 IEnumerable 建立 Queue</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素轉為 Queue 項目的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後佇列。</returns>
		public static Queue<TResult> ToQueue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			return source.Select(selector).ToQueue();
		}
		/// <summary>從 IEnumerable 建立 Queue</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素與索引轉為 Queue 項目的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">投影結果型別。</typeparam>
		/// <returns>投影後佇列。</returns>
		public static Queue<TResult> ToQueue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			return source.Select(selector).ToQueue();
		}



		/// <summary>source 如果為 null 就回傳 Empty</summary>
		/// <param name="source">可能為 `null` 的來源序列。</param>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <returns>原序列；若為 `null` 則回傳空序列。</returns>
		public static IEnumerable<T> NullToEmpty<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}


		/// <summary>加入項目到 IList</summary>
		/// <param name="source">要加入的來源項目序列。</param>
		/// <param name="collection">要接收元素的目標 `IList`。</param>
		public static void AddRangeTo(this IEnumerable source, IList collection)
		{
			foreach (var item in source) { collection.Add(item); }
		}

		/// <summary>加入項目到 ICollection&lt;T&gt;</summary>
		/// <param name="source">要加入的來源項目序列。</param>
		/// <param name="collection">要接收元素的目標泛型集合。</param>
		public static void AddRangeTo<T>(this IEnumerable<T> source, ICollection<T> collection)
		{
			foreach (var item in source) { collection.Add(item); }
		}

		/// <summary>一次加入多個項目到集合。</summary>
		/// <param name="target">要接收元素的目標集合。</param>
		/// <param name="value1">第一個要加入的元素。</param>
		/// <param name="value2">第二個要加入的元素。</param>
		/// <param name="values">其餘要加入的元素集合。</param>
		public static void Add<T>(this ICollection<T> target, T value1, T value2, params T[] values)
		{
			target.Add(value1);
			target.Add(value2);
			foreach (var value in values) { target.Add(value); }
		}



		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		/// <param name="enumeration">要逐一處理的來源序列。</param>
		/// <param name="action">對每個元素執行的動作。</param>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
			}
		}

		/// <summary> 賦予 IEnumerable ForEach Method</summary>
		/// <param name="enumeration">要逐一處理的來源序列。</param>
		/// <param name="action">對每個元素及其索引執行的動作。</param>
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
		{
			int i = 0;
			foreach (T item in enumeration)
			{
				action(item, i++);
			}
		}



		/// <summary> 賦予 IEnumerable Each Method</summary>
		/// <param name="enumeration">要逐一處理並回傳的來源序列。</param>
		/// <param name="action">對每個元素執行的動作。</param>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <returns>逐項執行動作後回傳原元素序列。</returns>
		public static IEnumerable<T> Each<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T item in enumeration)
			{
				action(item);
				yield return item;
			}
		}

		/// <summary> 賦予 IEnumerable Each Method</summary>
		/// <param name="enumeration">要逐一處理並回傳的來源序列。</param>
		/// <param name="action">對每個元素及其索引執行的動作。</param>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <returns>逐項執行動作後回傳原元素序列。</returns>
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
		/// <param name="enumeration">要串接的來源序列。</param>
		/// <param name="separator">連接各元素的分隔字元。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>串接字串；來源為 `null` 時回傳 `null`。</returns>
		public static string JoinBy<TSource>(this IEnumerable<TSource> enumeration, string separator)
		{
			if (enumeration == null) { return null; }
			return string.Join(separator, enumeration);
		}




		/// <summary>從 IEnumerable 建立 HashSet</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素轉為 HashSet 項目的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="T">目標元素型別。</typeparam>
		/// <returns>投影後集合。</returns>
		public static HashSet<T> ToHashSet<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> selector)
		{
			return source.Select(selector).ToHashSet();
		}
		/// <summary>從 IEnumerable 建立 HashSet</summary>
		/// <param name="source">要投影的來源序列。</param>
		/// <param name="selector">將元素與索引轉為 HashSet 項目的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="T">目標元素型別。</typeparam>
		/// <returns>投影後集合。</returns>
		public static HashSet<T> ToHashSet<TSource, T>(this IEnumerable<TSource> source, Func<TSource, int, T> selector)
		{
			return source.Select(selector).ToHashSet();
		}



		/// <summary> OrderBy 擴充，使用 bool 指定 descending， true Asc ; false Desc</summary>
		/// <typeparam name="TSource">來源型態</typeparam>
		/// <typeparam name="TKey">選擇欄位型態</typeparam>
		/// <param name="source">要排序的來源序列。</param>
		/// <param name="keySelector">用來取得排序鍵值的函式。</param>
		/// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
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
		/// <param name="source">前一次排序後的來源序列。</param>
		/// <param name="keySelector">用來取得次排序鍵值的函式。</param>
		/// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TKey">排序鍵值型別。</typeparam>
		/// <returns>追加次排序後的序列。</returns>
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool descending)
		{
			return descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
		}


		/// <summary>以列舉名稱對應屬性名稱進行排序。</summary>
		/// <param name="source">要排序的來源序列。</param>
		/// <param name="keySelector">代表屬性名稱的列舉值。</param>
		/// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>已排序序列。</returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Enum keySelector, bool descending)
		{
			return OrderBy(source, keySelector.ToString(), descending);
		}

		/// <summary>依屬性名稱動態排序序列。</summary>
		/// <param name="source">要排序的來源序列。</param>
		/// <param name="keySelector">要排序的屬性名稱。</param>
		/// <param name="descending">`true` 表示遞減排序；`false` 表示遞增排序。</param>
		/// <returns>已排序序列。</returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string keySelector, bool descending)
		{
			Type modelType = typeof(TSource);
			var prop = modelType.GetProperty(keySelector);
			if (prop == null) { throw new ArgumentOutOfRangeException(keySelector, "不存在"); }

			MethodInfo orderByMethod = Utils.EnumerableOrderByMethod(modelType, prop.PropertyType);
			LambdaExpression keyExpression = Utils.KeyExpression(modelType, prop);

			return (IOrderedEnumerable<TSource>)orderByMethod.Invoke(null, new object[]
			{
				source, keyExpression.Compile(), descending
			});
		}




		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		/// <param name="source">要比較的來源序列。</param>
		/// <typeparam name="TSource">元素型別（實值型別）。</typeparam>
		/// <returns>最大值；空集合時回傳預設值。</returns>
		public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MaxOrDefault(x => x);
		}

		/// <summary>傳回最大的結果值；如果找不到則傳回預設值。</summary>
		/// <param name="source">要比較的來源序列。</param>
		/// <param name="selector">將元素投影為可比較值的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">比較值型別（實值型別）。</typeparam>
		/// <returns>最大值；空集合時回傳預設值。</returns>
		public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MaxOrDefault(selector);
		}


		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		/// <param name="source">要比較的來源序列。</param>
		/// <typeparam name="TSource">元素型別（實值型別）。</typeparam>
		/// <returns>最小值；空集合時回傳預設值。</returns>
		public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source) where TSource : struct
		{
			return source.MinOrDefault(x => x);
		}

		/// <summary>傳回最小的結果值；如果找不到則傳回預設值。</summary>
		/// <param name="source">要比較的來源序列。</param>
		/// <param name="selector">將元素投影為可比較值的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TResult">比較值型別（實值型別）。</typeparam>
		/// <returns>最小值；空集合時回傳預設值。</returns>
		public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Expression<Func<TSource, TResult>> selector) where TResult : struct
		{
			return source.AsQueryable().MinOrDefault(selector);
		}








		/// <summary>計算 `int` 序列總和；空集合時回傳 0。</summary>
		/// <returns>`int` 總和。</returns>
		public static int SumOrDefault(this IEnumerable<int> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算 `long` 序列總和；空集合時回傳 0。</summary>
		/// <returns>`long` 總和。</returns>
		public static long SumOrDefault(this IEnumerable<long> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算 `float` 序列總和；空集合時回傳 0。</summary>
		/// <returns>`float` 總和。</returns>
		public static float SumOrDefault(this IEnumerable<float> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算 `double` 序列總和；空集合時回傳 0。</summary>
		/// <returns>`double` 總和。</returns>
		public static double SumOrDefault(this IEnumerable<double> source) { return source.SumOrDefault(x => x); }

		/// <summary>計算 `decimal` 序列總和；空集合時回傳 0。</summary>
		/// <returns>`decimal` 總和。</returns>
		public static decimal SumOrDefault(this IEnumerable<decimal> source) { return source.SumOrDefault(x => x); }



		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		/// <param name="source">要計算總和的來源序列。</param>
		/// <param name="selector">將元素投影為 `int` 的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>總和值；空集合時回傳 0。</returns>
		public static int SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, int>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		/// <param name="source">要計算總和的來源序列。</param>
		/// <param name="selector">將元素投影為 `long` 的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>總和值；空集合時回傳 0。</returns>
		public static long SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, long>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		/// <param name="source">要計算總和的來源序列。</param>
		/// <param name="selector">將元素投影為 `float` 的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>總和值；空集合時回傳 0。</returns>
		public static float SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, float>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		/// <param name="source">要計算總和的來源序列。</param>
		/// <param name="selector">將元素投影為 `double` 的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>總和值；空集合時回傳 0。</returns>
		public static double SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, double>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}
		/// <summary>計算數值序列的總和；空集合則傳回預設值。</summary>
		/// <param name="source">要計算總和的來源序列。</param>
		/// <param name="selector">將元素投影為 `decimal` 的運算式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>總和值；空集合時回傳 0。</returns>
		public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, decimal>> selector)
		{
			return source.AsQueryable().SumOrDefault(selector);
		}





		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的 `int` 序列。</param>
		/// <returns>`double` 平均值。</returns>
		public static double AverageOrDefault(this IEnumerable<int> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的 `long` 序列。</param>
		/// <returns>`double` 平均值。</returns>
		public static double AverageOrDefault(this IEnumerable<long> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的 `float` 序列。</param>
		/// <returns>`float` 平均值。</returns>
		public static float AverageOrDefault(this IEnumerable<float> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的 `double` 序列。</param>
		/// <returns>`double` 平均值。</returns>
		public static double AverageOrDefault(this IEnumerable<double> source)
		{
			return source.DefaultIfEmpty().Average();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的 `decimal` 序列。</param>
		/// <returns>`decimal` 平均值。</returns>
		public static decimal AverageOrDefault(this IEnumerable<decimal> source)
		{
			return source.DefaultIfEmpty().Average();
		}

		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的來源序列。</param>
		/// <param name="selector">將元素投影為 `int` 的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>平均值；空集合時回傳 0。</returns>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的來源序列。</param>
		/// <param name="selector">將元素投影為 `long` 的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>平均值；空集合時回傳 0。</returns>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的來源序列。</param>
		/// <param name="selector">將元素投影為 `float` 的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>平均值；空集合時回傳 0。</returns>
		public static float AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的來源序列。</param>
		/// <param name="selector">將元素投影為 `double` 的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>平均值；空集合時回傳 0。</returns>
		public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}
		/// <summary>計算序列的平均值；空集合則傳回預設值。</summary>
		/// <param name="source">要計算平均值的來源序列。</param>
		/// <param name="selector">將元素投影為 `decimal` 的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <returns>平均值；空集合時回傳 0。</returns>
		public static decimal AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			return source.Select(selector).AverageOrDefault();
		}





		/// <summary>將序列元素轉為指定型別，轉型失敗項目會略過。</summary>
		/// <param name="source">要嘗試轉型的來源序列。</param>
		/// <typeparam name="TResult">目標型別。</typeparam>
		/// <returns>轉型成功項目的序列。</returns>
		public static IEnumerable<TResult> Convert<TResult>(this IEnumerable source)
		{
			return Convert(source, typeof(TResult)).Cast<TResult>();
		}



		/// <summary>將序列元素轉為指定型別，轉型失敗項目會略過。</summary>
		/// <param name="source">要嘗試轉型的來源序列。</param>
		/// <param name="type">目標轉型型別。</param>
		/// <returns>轉型成功項目的序列。</returns>
		public static IEnumerable<object> Convert(this IEnumerable source, Type type)
		{
			foreach (var value in source)
			{
				object result = value.ConvertTo(type);
				if (result == null) { continue; }

				yield return result;
			}
		}




		/// <summary>依指定區塊大小將序列分段。</summary>
		/// <param name="source">要分段的來源序列。</param>
		/// <param name="blockSize">每個分段包含的元素數量。</param>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <returns>分段後的序列集合。</returns>
		public static IEnumerable<IEnumerable<T>> Bulk<T>(this IEnumerable<T> source, int blockSize)
		{
			if (source == null || !source.Any()) { return Enumerable.Empty<IEnumerable<T>>(); }

			return source
				.Select((value, i) => new { Value = value, Tag = i / blockSize })
				.GroupBy(x => x.Tag)
				.Select(g => g.Select(x => x.Value));
		}


		/// <summary>依指定區塊大小將序列分段並轉為清單。</summary>
		/// <param name="source">要分段的來源序列。</param>
		/// <param name="blockSize">每個分段包含的元素數量。</param>
		/// <typeparam name="T">元素型別。</typeparam>
		/// <returns>分段後的清單集合。</returns>
		public static IEnumerable<List<T>> BulkToList<T>(this IEnumerable<T> source, int blockSize)
		{
			return Bulk(source, blockSize).Select(x => x.ToList());
		}



		/// <summary>以廣度優先順序走訪節點與其子節點。</summary>
		/// <param name="items">要檢查的項目集合。</param>
		/// <param name="childSelector">取得目前節點子節點集合的函式。</param>
		/// <typeparam name="T">節點型別。</typeparam>
		/// <returns>廣度優先走訪結果。</returns>
		public static IEnumerable<T> Traversal<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
		{
			var queue = new Queue<T>(items);
			while (queue.Count > 0)
			{
				T item = queue.Dequeue();
				yield return item;

				IEnumerable<T> childs = childSelector(item);
				if (childs == null) { continue; }
				foreach (var child in childs) { queue.Enqueue(child); }
			}
		}



		/// <summary>預先建立索引並回傳依鍵值取回清單的函式。</summary>
		/// <param name="source">要建立索引的來源序列。</param>
		/// <param name="keySelector">決定分組鍵值的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TKey">鍵值型別。</typeparam>
		/// <returns>輸入鍵值後回傳對應元素清單的函式。</returns>
		public static Func<TKey, List<TSource>> PrepareList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return PrepareList(source, keySelector, x => x);
		}
		/// <summary>預先建立索引並回傳依鍵值取回投影清單的函式。</summary>
		/// <param name="source">要建立索引的來源序列。</param>
		/// <param name="keySelector">決定分組鍵值的函式。</param>
		/// <param name="valueSelector">將來源元素轉為輸出清單項目的函式。</param>
		/// <typeparam name="TSource">來源元素型別。</typeparam>
		/// <typeparam name="TKey">鍵值型別。</typeparam>
		/// <typeparam name="TValue">輸出值型別。</typeparam>
		/// <returns>輸入鍵值後回傳對應投影清單的函式。</returns>
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

