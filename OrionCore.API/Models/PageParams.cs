using System;
using System.Linq.Expressions;
using Orion.Api.Extensions;

namespace Orion.Api.Models
{
	/// <summary>查詢分頁與排序設定。</summary>
	public class PageParams
	{
		/// <summary>頁碼索引（通常由 1 開始）。</summary>
		public int PageIndex { get; set; }

		/// <summary>每頁筆數。</summary>
		public int PageSize { get; set; }

		/// <summary>排序欄位字串，可用前綴 <c>-</c> 表示遞減排序（例如 <c>-CREATE_DT</c>）。</summary>
		public string OrderField { get; set; }

	}


	/// <summary>`PageParams` 的擴充方法。</summary>
	public static class PageParamsExtensions
	{
		/// <summary>將可能為 <c>null</c> 的分頁設定轉為可用實例。</summary>
		/// <param name="source">來源資料。</param>
		/// <returns>來源不為 <c>null</c> 時回傳原值，否則回傳 <c>PageSize = -1</c> 的不限筆數設定。</returns>
		public static PageParams NullToUnlimited(this PageParams source)
		{
			return source ?? new PageParams { PageSize = -1 };
		}

		/// <summary>設定排序欄位字串，支援以 `-` 表示遞減排序。</summary>
		/// <param name="source">分頁參數。</param>
		/// <param name="orderField">排序欄位名稱。</param>
		/// <param name="descending">是否遞減排序。</param>
		public static void SetOrderField(this PageParams source, string orderField, bool descending)
		{
			source.OrderField = descending ? "-" + orderField : orderField;
		}

	}


}
