using System;
using System.Linq.Expressions;
using Orion.Api.Extensions;

namespace Orion.Api.Models
{
	/// <summary>分頁參數</summary>
	public class PageParams
	{
		/// <summary></summary>
		public int PageIndex { get; set; }

		/// <summary></summary>
		public int PageSize { get; set; }

		/// <summary></summary>
		public string OrderField { get; set; }

	}


	public static class PageParamsExtensions 
	{
		/// <summary>PageParams 如果為 null 就回傳不限制分頁 Unlimited</summary>
		public static PageParams NullToUnlimited(this PageParams source)
		{
			return source ?? new PageParams { PageSize = -1 };
		}

		/// <summary></summary>
		public static void SetOrderField(this PageParams source, string orderField, bool descending)
		{
			source.OrderField = descending ? "-" + orderField : orderField;
		}

	}


}
