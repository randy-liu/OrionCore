using System;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrionCore.API.Extensions;

namespace OrionCore.Mvc.Attributes
{
	/// <summary></summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class SearchRememberAttribute : ActionFilterAttribute
	{
        private static string _storeName = "sr"; 

        private static string[] _skipKey = { 
            "PageIndex", 
            "PageSize",  
        };


		/// <summary></summary>
		public string StoreKeyParam { get; set; }

		/// <summary></summary>
		public string[] StoreOnly { get; set; } = new string[0];

		/// <summary></summary>
		public string[] Ignore { get; set; } = new string[0];

		/// <summary></summary>
		public SearchRememberAttribute() { }


		/// <summary></summary>
		public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            base.OnActionExecuting(filterContext);
			var controller = filterContext.Controller as Controller;


			/* 略過重導向 */
			if (controller.TempData.ContainsKey("Redirected"))
			{ 
				controller.TempData.Remove("Redirected"); 
				return; 
			}


			var request = filterContext.HttpContext.Request;

            /* 複製 QueryString */
            var qs = HttpUtility.ParseQueryString(request.QueryString.ToString());
			string storeName = qs[StoreKeyParam] ?? _storeName;
			qs.Remove(StoreKeyParam);


			/* 移除不紀錄的參數 */
			if (StoreOnly.Length > 0)
			{
				foreach (string key in qs.AllKeys)
				{

					if (StoreOnly.Contains(key)) { continue; }
					if (_skipKey.Contains(key)) { continue; }
					qs.Remove(key);
				}
			}

			/* 移除忽略的參數 */
			foreach (string key in Ignore) { qs.Remove(key); }


			/* 有參數則記錄 */
			if (qs.Count > 0)
            {
				foreach (var key in _skipKey) { qs.Remove(key); }

				IResponseCookies cookies = filterContext.HttpContext.Response.Cookies;

				cookies.Append(storeName, qs.ToString(), new CookieOptions
				{
					Path = request.Path,
					Expires = DateTime.Now.AddYears(1)
				});

                return; 
            }

			/* 取回記錄中的參數 */
			string cookieValue = request.Cookies[storeName];
			if (cookieValue.NoText()) { return; }

			var store = HttpUtility.ParseQueryString(cookieValue);
			foreach (var pair in request.Query) { store[pair.Key] = pair.Value; }

			controller.TempData["Redirected"] = true;
			filterContext.Result = new RedirectResult(request.Path + "?" + store.ToString());
        }

    }
}
