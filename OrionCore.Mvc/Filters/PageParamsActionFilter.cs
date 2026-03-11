using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Api.Models;

namespace Orion.Mvc.Filters
{
    /// <summary>PageParams 的預設值 ActionFilter</summary>
    public class PageParamsActionFilter : ActionFilterAttribute
	{
		private readonly string _sizeParam;
		private readonly int _defaultSize;

		/// <summary>建立分頁參數預設值過濾器。</summary>
		/// <param name="sizeParam">頁面大小對應的 Query/Cookie 參數名稱。</param>
		/// <param name="defaultSize">預設頁面大小。</param>
		public PageParamsActionFilter(string sizeParam, int defaultSize)
		{
			_sizeParam = sizeParam;
			_defaultSize = defaultSize;
		}


		/// <summary>在 Action 執行前套用 `PageParams.PageSize` 預設值並同步 Cookie。</summary>
		/// <param name="filterContext">目前 Action 執行內容。</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			PageParams pageParams = filterContext.ActionArguments.Values.OfType<PageParams>().FirstOrDefault();
			if (pageParams == null) { base.OnActionExecuting(filterContext); return; }


			var request = filterContext.HttpContext.Request;
			var response = filterContext.HttpContext.Response;

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
				response.Cookies.Append(_sizeParam, pageSize, new CookieOptions { Expires = DateTime.Now.AddYears(1) });
			}

			var controller = filterContext.Controller as Controller;
			controller.ViewData[_sizeParam] = pageParams.PageSize;
			base.OnActionExecuting(filterContext);
		}

	}
}
