using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.Api.Extensions
{
	/// <summary>對於需要實作 IEqualityComparer 的 Extension Method 進行的 Extension 定義</summary>
	public static class EqualityComparerExtensions
	{


		/// <summary>使用自訂相等判斷函式檢查序列是否包含指定元素。</summary>
		/// <param name="source">要搜尋元素的來源序列。</param>
		/// <param name="value">要判斷是否存在於序列中的元素。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <returns>找到符合比較條件的元素時回傳 `true`。</returns>
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> comparer)
		{
			return source.Contains(value, new LambdaComparer<TSource>(comparer));
		}

		/// <summary>使用自訂相等判斷函式移除重複元素。</summary>
		/// <param name="source">要去除重複元素的來源序列。</param>
		/// <param name="comparer">比較兩個元素是否視為相同的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <returns>去除重複後的序列。</returns>
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> comparer)
		{
			return source.Distinct(new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式，回傳存在於第一序列但不在第二序列的元素。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要從 `first` 排除的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <returns>差集結果序列。</returns>
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Except(second, new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式，回傳兩序列交集元素。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要與 `first` 取交集的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <returns>交集結果序列。</returns>
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Intersect(second, new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式合併兩序列並去除重複。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要與 `first` 合併的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <returns>合併後序列。</returns>
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return first.Union(second, new LambdaComparer<TSource>(comparer));
		}



		/// <summary>使用自訂相等判斷函式檢查查詢是否包含指定元素。</summary>
		/// <param name="source">要搜尋元素的來源查詢。</param>
		/// <param name="item">要判斷是否存在於查詢中的元素。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">查詢元素型別。</typeparam>
		/// <returns>找到符合比較條件的元素時回傳 `true`。</returns>
		public static bool Contains<TSource>(this IQueryable<TSource> source, TSource item, Func<TSource, TSource, bool> comparer)
		{
			return source.Contains(item, new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式移除查詢中的重複元素。</summary>
		/// <param name="source">要去除重複元素的來源查詢。</param>
		/// <param name="comparer">比較兩個元素是否視為相同的函式。</param>
		/// <typeparam name="TSource">查詢元素型別。</typeparam>
		/// <returns>去除重複後的查詢。</returns>
		public static IQueryable<TSource> Distinct<TSource>(this IQueryable<TSource> source, Func<TSource, TSource, bool> comparer)
		{
			return source.Distinct(new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式，回傳查詢與序列的差集。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要從 `source1` 排除的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>差集結果查詢。</returns>
		public static IQueryable<TSource> Except<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Except(source2, new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式，回傳查詢與序列的交集。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要與 `source1` 取交集的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>交集結果查詢。</returns>
		public static IQueryable<TSource> Intersect<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Intersect(source2, new LambdaComparer<TSource>(comparer));
		}
		/// <summary>使用自訂相等判斷函式合併查詢與序列並去除重複。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要與 `source1` 合併的元素序列。</param>
		/// <param name="comparer">比較兩個元素是否相等的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <returns>合併後查詢。</returns>
		public static IQueryable<TSource> Union<TSource>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TSource, bool> comparer)
		{
			return source1.Union(source2, new LambdaComparer<TSource>(comparer));
		}



		/// <summary>
		/// 外面不需要知道這個類的存在，就算提供也不好用。
		/// </summary>
		internal class LambdaComparer<TSource> : IEqualityComparer<TSource>
		{

			private readonly Func<TSource, TSource, bool> _comparer;

			/// <summary>以自訂比較函式建立比較器。</summary>
			/// <param name="comparer">比較兩個 `TSource` 是否相等的函式。</param>
			public LambdaComparer(Func<TSource, TSource, bool> comparer)
			{
				_comparer = comparer;
			}

			/// <summary>比較兩個物件是否相等。</summary>
			/// <param name="x">第一個比較值。</param>
			/// <param name="y">第二個比較值。</param>
			/// <returns>相等時回傳 `true`。</returns>
			public bool Equals(TSource x, TSource y)
			{
				return _comparer(x, y);
			}

			/// <summary>取得雜湊碼。</summary>
			/// <param name="obj">要取得雜湊碼的物件。</param>
			/// <returns>固定回傳 0。</returns>
			public int GetHashCode(TSource obj)
			{
				return 0;
			}
		}






		/// <summary>依指定鍵值函式檢查序列是否包含指定元素。</summary>
		/// <param name="source">要搜尋元素的來源序列。</param>
		/// <param name="value">要判斷是否存在於序列中的元素。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>找到鍵值相同的元素時回傳 `true`。</returns>
		public static bool Contains<TSource, TProperty>(this IEnumerable<TSource> source, TSource value, Func<TSource, TProperty> getter)
		{
			return source.Contains(value, new LambdaComparer<TSource, TProperty>(getter));
		}

		/// <summary>依指定鍵值函式移除重複元素。</summary>
		/// <param name="source">要去除重複元素的來源序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>去除重複後的序列。</returns>
		public static IEnumerable<TSource> Distinct<TSource, TProperty>(this IEnumerable<TSource> source, Func<TSource, TProperty> getter)
		{
			return source.Distinct(new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式，回傳第一序列相對第二序列的差集。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要從 `first` 排除的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>差集結果序列。</returns>
		public static IEnumerable<TSource> Except<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Except(second, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式，回傳兩序列交集元素。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要與 `first` 取交集的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>交集結果序列。</returns>
		public static IEnumerable<TSource> Intersect<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Intersect(second, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式合併兩序列並去除重複。</summary>
		/// <param name="first">主要來源序列。</param>
		/// <param name="second">要與 `first` 合併的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">序列元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>合併後序列。</returns>
		public static IEnumerable<TSource> Union<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> getter)
		{
			return first.Union(second, new LambdaComparer<TSource, TProperty>(getter));
		}



		/// <summary>依指定鍵值函式檢查查詢是否包含指定元素。</summary>
		/// <param name="source">要搜尋元素的來源查詢。</param>
		/// <param name="item">要判斷是否存在於查詢中的元素。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">查詢元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>找到鍵值相同的元素時回傳 `true`。</returns>
		public static bool Contains<TSource, TProperty>(this IQueryable<TSource> source, TSource item, Func<TSource, TProperty> getter)
		{
			return source.Contains(item, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式移除查詢中的重複元素。</summary>
		/// <param name="source">要去除重複元素的來源查詢。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">查詢元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>去除重複後的查詢。</returns>
		public static IQueryable<TSource> Distinct<TSource, TProperty>(this IQueryable<TSource> source, Func<TSource, TProperty> getter)
		{
			return source.Distinct(new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式，回傳查詢與序列的差集。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要從 `source1` 排除的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>差集結果查詢。</returns>
		public static IQueryable<TSource> Except<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Except(source2, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式，回傳查詢與序列的交集。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要與 `source1` 取交集的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>交集結果查詢。</returns>
		public static IQueryable<TSource> Intersect<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Intersect(source2, new LambdaComparer<TSource, TProperty>(getter));
		}
		/// <summary>依指定鍵值函式合併查詢與序列並去除重複。</summary>
		/// <param name="source1">主要來源查詢。</param>
		/// <param name="source2">要與 `source1` 合併的元素序列。</param>
		/// <param name="getter">從元素取出比較鍵值的函式。</param>
		/// <typeparam name="TSource">元素型別。</typeparam>
		/// <typeparam name="TProperty">比較鍵值型別。</typeparam>
		/// <returns>合併後查詢。</returns>
		public static IQueryable<TSource> Union<TSource, TProperty>(this IQueryable<TSource> source1, IEnumerable<TSource> source2, Func<TSource, TProperty> getter)
		{
			return source1.Union(source2, new LambdaComparer<TSource, TProperty>(getter));
		}



		/// <summary>
		/// 外面不需要知道這個類的存在，就算提供也不好用。
		/// </summary>
		internal class LambdaComparer<TSource, TProperty> : IEqualityComparer<TSource>
		{

			private readonly Func<TSource, TProperty> _getter;

			/// <summary>以鍵值取得函式建立比較器。</summary>
			/// <param name="getter">從元素取出比較鍵值的函式。</param>
			public LambdaComparer(Func<TSource, TProperty> getter)
			{
				_getter = getter;
			}

			/// <summary>比較兩個物件的鍵值是否相等。</summary>
			/// <param name="x">第一個比較值。</param>
			/// <param name="y">第二個比較值。</param>
			/// <returns>鍵值相等時回傳 `true`。</returns>
			public bool Equals(TSource x, TSource y)
			{
				TProperty xValue = _getter(x);
				TProperty yValue = _getter(y);
				if (xValue == null || yValue == null) { return false; }

				return xValue.Equals(yValue);
			}

			/// <summary>取得雜湊碼。</summary>
			/// <param name="obj">要取得雜湊碼的物件。</param>
			/// <returns>固定回傳 0。</returns>
			public int GetHashCode(TSource obj)
			{
				return 0;
			}
		}

	}


}
