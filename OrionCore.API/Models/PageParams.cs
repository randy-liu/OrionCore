using OrionCore.API.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace OrionCore.API.Models
{
	/// <summary>分頁參數</summary>
	public interface IPageParams
	{
		/// <summary></summary>
		int PageIndex { get; set; }

		/// <summary></summary>
		int PageSize { get; set; }

		/// <summary></summary>
		string OrderField { get; set; }

	}



	/// <summary>分頁參數</summary>
	public class PageParams<T> : IPageParams
	{

		/// <summary>不限制 PageSize</summary>
        [Obsolete("請使用 pageParams = pageParams.NullToUnlimited();")]
		public static PageParams<T> Unlimited() { return new PageParams<T> { PageSize = -1 }; }

		/// <summary></summary>
		public int PageIndex { get; set; }

		/// <summary></summary>
		public int PageSize { get; set; }

		/// <summary></summary>
		public string OrderField { get; set; }



		/// <summary></summary>
		public void SetOrderField<TProp>(Expression<Func<T, TProp>> selector)
		{
			OrderField = selector.GetProperty().Name;
		}

		/// <summary></summary>
		public void SetOrderField(T selector)
		{
			OrderField = selector.ToString();
		}

	}
}
