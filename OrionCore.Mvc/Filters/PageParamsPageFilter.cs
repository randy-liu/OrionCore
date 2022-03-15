using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.Api.Models;

namespace Orion.Mvc.Filters
{

	/// <summary>PageParams 的預設值 ActionFilter</summary>
	public class PageParamsPageFilter : AbstractPageFilter
	{
		private readonly string _sizeParam;
		private readonly int _defaultSize;

		/// <summary></summary>
		public PageParamsPageFilter(string sizeParam, int defaultSize)
		{
			_sizeParam = sizeParam;
			_defaultSize = defaultSize;
		}


		/// <summary></summary>
		public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			PageParams pageParams = context.HandlerArguments.Values.OfType<PageParams>().FirstOrDefault();
			if (pageParams == null) { return; }


			var request = context.HttpContext.Request;
			var response = context.HttpContext.Response;

			string cookieValue = request.Cookies[_sizeParam];
			string pageSize = pageParams.PageSize.ToString();

			if (pageParams.PageSize == 0)
			{
				try
				{ pageParams.PageSize = int.Parse(cookieValue); }
				catch
				{ pageParams.PageSize = _defaultSize; }
			}
			else if (cookieValue != pageSize)
			{
				response.Cookies.Append(_sizeParam, pageSize, new CookieOptions 
				{ 
					Path = request.Path,
					Expires = DateTime.Now.AddYears(1),
				});
			}

			var page = context.HandlerInstance as PageModel;
			if (page == null) { return; }

			page.ViewData[_sizeParam] = pageParams.PageSize;
		}


	}

}
