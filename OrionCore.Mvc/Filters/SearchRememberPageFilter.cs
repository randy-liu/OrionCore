using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.Api.Extensions;
using Orion.Mvc.Attributes;

namespace Orion.Mvc.Filters
{
    /// <summary></summary>
    public class SearchRememberPageFilter : AbstractPageFilter
    {
        private readonly ConcurrentDictionary<MethodInfo, SearchRememberAttribute> _cache = new ConcurrentDictionary<MethodInfo, SearchRememberAttribute>();

        private readonly string _storeName = "sr";
        private readonly string[] _skipKey;

        /// <summary></summary>
        public SearchRememberPageFilter(string[] skipKey)
        {
            _skipKey = skipKey;
        }


        /// <summary></summary>
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context.HandlerMethod == null) { return; }

            var attr = _cache.GetOrAdd(context.HandlerMethod.MethodInfo, m => m.GetCustomAttribute<SearchRememberAttribute>());
            if (attr == null) { return; }

            var page = context.HandlerInstance as PageModel;
            if (page == null) { return; }

            /* 略過重導向 */
            if (page.TempData.ContainsKey("Redirected"))
            {
                page.TempData.Remove("Redirected");
                return;
            }

            HttpRequest request = context.HttpContext.Request;

            /* 複製 QueryString */
            var qs = HttpUtility.ParseQueryString(request.QueryString.Value);

            string storeName = qs[attr.StoreKeyParam] ?? _storeName;
            qs.Remove(attr.StoreKeyParam);


            /* 移除不紀錄的參數 */
            if (attr.StoreOnly.Length > 0)
            { qs.AllKeys.Except(attr.StoreOnly).ForEach(qs.Remove); }

            /* 移除忽略的參數 */
            attr.Ignore.ForEach(qs.Remove);


            /* 有參數則記錄 */
            if (qs.Count > 0)
            {
                _skipKey.ForEach(qs.Remove);
                var options = new CookieOptions { Path = request.Path, Expires = DateTime.Now.AddYears(1) };
                context.HttpContext.Response.Cookies.Append(storeName, qs.ToString(), options);
                return;
            }


            /* 取回記錄中的參數 */
            string cookieValue = request.Cookies[_storeName];
            if (cookieValue.NoText()) { return; }

            var store = HttpUtility.ParseQueryString(cookieValue);
            foreach (var pair in request.Query) { store[pair.Key] = pair.Value; }

            page.TempData["Redirected"] = true;
            context.Result = new RedirectResult(request.Path + "?" + store.ToString().TrimStart('?'));
        }


    }

}
