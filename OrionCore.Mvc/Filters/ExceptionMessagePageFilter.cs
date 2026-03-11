using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orion.Api;
using Orion.Mvc.Extensions;

namespace Orion.Mvc.Filters
{

    /// <summary>Razor Page 例外訊息過濾器，將指定例外轉為友善回應。</summary>
    public class ExceptionMessagePageFilter : AbstractPageFilter
    {
        /// <summary>在 Page Handler 執行後統一處理 `UserException`。</summary>
        /// <param name="context">目前 Page Handler 執行結果內容。</param>
        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            if (context.ExceptionHandled) { return; }

            /* 判斷是否有指定的 Exception */
            var ex = context.Exception;
            if (ex is UserException userEx) { handleUserException(context, userEx); return; }
            //if (ex is HttpException httpEx) { handleHttpException(context, httpEx); return; }
            //if (ex is OracleException oracleEx) { handleOracleException(context, oracleEx); return; }
        }


        /// <summary>將 `UserException` 轉為 Razor Page 可用的回應結果。</summary>
        /// <param name="context">目前 Page Handler 執行結果內容。</param>
        /// <param name="ex">要處理的使用者例外。</param>
        private void handleUserException(PageHandlerExecutedContext context, UserException ex)
        {
            /* 只針對 PageModel 進行錯誤處理*/
            var page = context.HandlerInstance as PageModel;
            if (page == null) { return; }

            /* 標記 Exception 已經被處理了，讓後續的 Filter 不用再處理 */
            context.ExceptionHandled = true;

            if (context.HttpContext.Request.IsAjaxRequest())
            {
                /* 對 Ajax 請求的處理 */
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    StatusCode = 400,
                    Content = ex.Message
                };
            }
            else if (ex is UserNoDataException)
            {
                /* 資料不存在的處理 */
                page.TempData["StatusError"] = ex.Message;
                context.HttpContext.Response.StatusCode = 404;
            }
            else
            {
                /* 一般畫面的處理 */
                page.TempData["StatusError"] = ex.Message;
                context.Result = page.Page();
            }
        }



        //private void handleHttpException(PageHandlerExecutedContext context, HttpException ex)
        //{
        //    context.ExceptionHandled = true;

        //    if (context.HttpContext.Request.IsAjaxRequest())
        //    {
        //        context.HttpContext.Response.StatusCode = ex.StatusCode;
        //        context.Result = new ContentResult
        //        {
        //            StatusCode = ex.StatusCode,
        //            Content = ex.Message,
        //        };
        //    }
        //    else
        //    {
        //        context.HttpContext.Response.StatusCode = ex.StatusCode;
        //        context.Result = new StatusCodeResult(ex.StatusCode);
        //    }
        //}


        //private void handleOracleException(PageHandlerExecutedContext context, OracleException ex)
        //{
        //    /* 判斷是否為自訂錯誤 */
        //    if (ex.Number != 20019) { return; }
        //    handleUserException(context, new UserException(ex.Message.Split('\n').First()));
        //}



    }
}
