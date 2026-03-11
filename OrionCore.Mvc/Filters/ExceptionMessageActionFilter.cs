using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Orion.Api;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Filters
{
    /// <summary>例外訊息過濾器</summary>
    public class ExceptionMessageActionFilter : ActionFilterAttribute
    {

        /// <summary>在 Action 執行前將主要 ViewModel/Domain 物件寫入 `ViewData.Model`。</summary>
        /// <param name="filterContext">目前 Action 執行內容。</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /* 自動將 Action 的 Arguments 中的 ViewModel 賦予給 ViewData
			 * 不然要自己在 Action 的第一行寫 ViewData.Model = domain;
			 */
            var arguments = filterContext.ActionArguments.Values.Where(x => x != null);

            object model = arguments
                .OrderBy(x => Regex.IsMatch(x.GetType().Name, @"(ViewModel|Domain)\b") ? 0 : 1)
                .FirstOrDefault();

            var controller = filterContext.Controller as Controller;
            controller.ViewData.Model = model;
        }


        /// <summary>在 Action 執行後統一處理 `UserException` 並轉為對應結果。</summary>
        /// <param name="filterContext">目前 Action 執行結果內容。</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            /* 判斷 Exception 是否已經被處理了 */
            base.OnActionExecuted(filterContext);
            if (filterContext.ExceptionHandled) { return; }


            /* 只針對 UserException 進行錯誤處理*/
            var ex = filterContext.Exception as UserException;
            if (ex == null) { return; }

            /* 標記 Exception 已經被處理了，讓後續的 Filter 不用再處理 */
            filterContext.ExceptionHandled = true;


            var controller = filterContext.Controller as Controller;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                /* 對 Ajax 請求的處理 */
                filterContext.HttpContext.Response.StatusCode = 400;
                filterContext.Result = new ContentResult { StatusCode = 400, Content = ex.Message };
            }
            else if (ex is UserNoDataException)
            {
                /* 資料不存在的處理 */
                controller.TempData["StatusError"] = ex.Message;
                filterContext.HttpContext.Response.StatusCode = 404;
            }
            else
            {
                /* 一般畫面的處理 */
                controller.TempData["StatusError"] = ex.Message;
                filterContext.Result = new ViewResult
                {
                    ViewData = controller.ViewData,
                    TempData = controller.TempData
                };
            }
        }

    }
}