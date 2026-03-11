using System;
using System.Collections.Generic;
using System.Linq;
using Orion.Api.Models;

namespace Orion.Api.Extensions
{
    /// <summary>定義 Pagination&lt;TSource&gt; 的 Extension</summary>
    public static class PaginationExtensions
    {

        /// <summary>預設換頁大小</summary>
        public static int DefaultPageSize { get; set; } = 20;


        /// <summary>將列舉集合轉為分頁物件。</summary>
        /// <typeparam name="T">元素型別。</typeparam>
        /// <param name="source">要分頁的來源集合。</param>
		/// <param name="pageNumber">要求的頁碼；小於 1 會自動調整為 1。</param>
		/// <param name="pageSize">每頁筆數；`-1` 表示回傳全部資料，`0` 或負值（不含 `-1`）會改用 `DefaultPageSize`。</param>
        /// <returns>計算後的分頁結果。</returns>
        public static Pagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            IQueryable<T> query = source.AsQueryable();
            return AsPagination(query, pageNumber, pageSize);
        }


        /// <summary>將查詢物件轉為分頁結果。</summary>
        /// <typeparam name="T">元素型別。</typeparam>
		/// <param name="query">要分頁的來源查詢。</param>
		/// <param name="pageNumber">要求的頁碼；小於 1 會自動調整為 1，超過總頁數會調整為最後一頁。</param>
		/// <param name="pageSize">每頁筆數；`-1` 表示回傳全部資料，`0` 或負值（不含 `-1`）會改用 `DefaultPageSize`。</param>
        /// <returns>計算後的分頁結果。</returns>
        public static Pagination<T> AsPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var result = new Pagination<T>
            {
                PageNumber = Math.Max(1, pageNumber),
                PageSize = pageSize,
            };

            if (pageSize == -1)
            {
                result.List = query.ToList();
                result.TotalItems = result.List.Count;
                return result;
            }

            result.TotalItems = query.Count();
            if (result.TotalItems == 0)
            {
                result.List = new List<T>();
                return result;
            }

            if (result.PageSize <= 0) { result.PageSize = DefaultPageSize; }
            int totalPages = (int)Math.Ceiling(((double)result.TotalItems) / result.PageSize);
            result.PageNumber = Math.Min(result.PageNumber, totalPages);

            int skip = (result.PageNumber - 1) * result.PageSize;
            result.List = query.Skip(skip).Take(result.PageSize).ToList();

            return result;
        }



        /// <summary>將分頁內容投影為另一種型別並保留分頁資訊。</summary>
        /// <typeparam name="TSource">來源項目型別。</typeparam>
        /// <typeparam name="TResult">目標項目型別。</typeparam>
		/// <param name="src">要進行投影的來源分頁物件。</param>
		/// <param name="selector">將來源項目轉換為目標型別的函式。</param>
        /// <returns>投影後的分頁結果。</returns>
        public static Pagination<TResult> As<TSource, TResult>(this Pagination<TSource> src, Func<TSource, TResult> selector)
        {
            var pagination = new Pagination<TResult>
            {
                List = src.List.Select(selector).ToList(),
                PageNumber = src.PageNumber,
                PageSize = src.PageSize,
                TotalItems = src.TotalItems
            };

            return pagination;
        }





    }
}
