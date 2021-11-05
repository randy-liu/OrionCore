using OrionCore.API.Models;
using OrionCore.API.Extensions;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrionCore.Mvc.Html
{

	/// <summary></summary>
	public static class PaginationExtensions
	{
		/// <summary></summary>
		public static string IndexParam { get; set; } = "pageIndex";
		/// <summary></summary>
		public static string SortParam { get; set; } = "orderField";


		/// <summary>換頁的頁面連結列</summary>
		/// <param name="helper">The HTML Helper</param>
		/// <param name="pagination">The datasource</param>
		public static IHtmlContent PagerLinks(this IHtmlHelper helper, IPagination pagination)
		{
			return PagerLinks(helper, pagination, IndexParam);
		}

		/// <summary>換頁的頁面連結列</summary>
		/// <param name="helper">The HTML Helper</param>
		/// <param name="pagination">The datasource</param>
		/// <param name="param">連結的 QueryString 名稱</param>
		public static IHtmlContent PagerLinks(this IHtmlHelper helper, IPagination pagination, string param)
		{
			/*資料為空*/
			if (pagination.TotalItems == 0)
			{
				return new HtmlString(String.Empty);
			}

			string queryString = helper.ViewContext.HttpContext.Request.QueryString.ToString();
			NameValueCollection values = HttpUtility.ParseQueryString(queryString);
			values.Remove(param);
			values.Add(param, "");

			string linkTpl = "<li class=\"{0}\"><a href=\"?" + values.ToString() + "{2}\">{1}</a></li>";
			string spanTpl = "<li class=\"{0}\"><span>{1}</span></li>";

			/*連結列*/
			var links = new StringBuilder();

			int index = pagination.PageNumber;
			int total = pagination.TotalPages;

			int smL = index - 2;
			int smR = index + 2;
			if (smR >= total) { smL -= (smR - total); }
			if (smL <= 1) { smR -= (smL - 1); }

			int max = 11;
			int center = (max + 1) / 2; //6
			int romL;
			int romR;

			if (total < max) /*頁面小於 limit*/
			{
				romL = 2;
				romR = total - 1;
			}
			else if ((index - center) < 0) /*左側接近最小值*/
			{
				romL = 2;
				romR = max - 1;
			}
			else if ((index + center) > total) /*右側接近最大值*/
			{
				romL = total - max + 2;
				romR = total - 1;/*romL=t-(m-1)*/
			}
			else /*中間滑動*/
			{
				romL = index - center + 2;
				romR = index + center - 2;
			}



			if (pagination.HasPreviousPage)
			{
				links.AppendFormat(linkTpl, "", "&laquo;", index - 1);
			}
			else
			{
				links.AppendFormat(spanTpl, "disabled", "&laquo;");
			}


			/*第一個頁面連結*/
			if (index == 1)
			{
				links.AppendFormat(spanTpl, "active", 1);
			}
			else
			{
				links.AppendFormat(linkTpl, "", 1, 1);
			}

			/* ... */
			if (index > 2)
			{
				string className = "disabled";
				if (total < max) { className += " hidden-lg"; }
				if (romL <= 2) { className += " hidden-md"; }
				if (index <= 4) { className += " hidden-sm"; }
				links.AppendFormat(spanTpl, className, "...");
			}

			/*中間的頁面連結*/
			for (int i = romL; i <= romR; i++)
			{
				if (i == index)
				{
					links.AppendFormat(spanTpl, "active", i);
				}
				else
				{
					string className = "hidden-xs";
					if (i < smL || i > smR) { className += " hidden-sm"; }
					links.AppendFormat(linkTpl, className, i, i);
				}
			}

			/* ... */
			if ((total - index) >= 2)
			{
				string className = "disabled";
				if (total < max) { className += " hidden-lg"; }
				if ((total - romR) < 2) { className += " hidden-md"; }
				if ((total - index) < 4) { className += " hidden-sm"; }
				links.AppendFormat(spanTpl, className, "...");
			}

			/*最後一個頁面連結*/
			if (total > 1)
			{
				if (index == total)
				{
					links.AppendFormat(spanTpl, "active", total);
				}
				else
				{
					links.AppendFormat(linkTpl, "", total, total);
				}
			}

			if (pagination.HasNextPage)
			{
				links.AppendFormat(linkTpl, "", "&raquo;", index + 1);
			}
			else
			{
				links.AppendFormat(spanTpl, "disabled", "&raquo;");
			}

			/*
			  <li class=""><a href="#">&laquo;</a></li>
			  <li class=""><a href="#">1</a></li>
			  <li class=""><a href="#">2</a></li>
			  <li class=""><a href="#">&raquo;</a></li>
			 */
			return new HtmlString(links.ToString());
		}




		/// <summary></summary>
		public static IHtmlContent PagerSortFor<TModel, TProp>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProp>> expression)
		{
			return PagerSortFor(helper, expression, null);
		}
		/// <summary></summary>
		public static IHtmlContent PagerSortFor<TModel, TProp>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TProp>> expression, string name)
		{
			PropertyInfo prop = expression.GetProperty();
			if (name.NoText()) { name = prop.GetDisplayName(); }
			if (name.NoText()) { name = prop.Name; }

			return PagerSort(helper, prop.Name, name);
		}



		/// <summary></summary>
		public static IHtmlContent PagerSort(this IHtmlHelper helper, string column, string name)
		{
			string queryString = helper.ViewContext.HttpContext.Request.QueryString.Value;
			NameValueCollection values = HttpUtility.ParseQueryString(queryString);

			string iconName = "fa-sort";

			if (values[SortParam] == ("-" + column))
			{
				values[SortParam] = column;
				iconName = "fa-sort-desc";
			}
			else if (values[SortParam] == column)
			{
				values[SortParam] = "-" + column;
				iconName = "fa-sort-asc";
			}
			else
			{
				values[SortParam] = column;
			}


			string html = string.Format("<a href=\"?{0}\">{1}<i class=\"fa fa-fw {2}\"></i></a>",
				values.ToString(), HttpUtility.HtmlEncode(name), iconName
			);

			return new HtmlString(html);
		}

	}

}