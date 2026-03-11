using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Orion.Mvc.Extensions
{
    /// <summary>提供 `Controller` 與 `PageModel` 的狀態訊息設定擴充方法。</summary>
    public static class ControllerExtensions
    {

        /// <summary>設定 `TempData["StatusSuccess"]` 狀態訊息。</summary>
        /// <param name="controller">目前 Controller。</param>
        /// <param name="message">要顯示的成功訊息。</param>
        public static void SetStatusSuccess(this Controller controller, string message)
        {
            controller.TempData["StatusSuccess"] = message;
        }


        /// <summary>設定 `TempData["StatusError"]` 狀態訊息。</summary>
        /// <param name="controller">目前 Controller。</param>
        /// <param name="message">要顯示的錯誤訊息。</param>
        public static void SetStatusError(this Controller controller, string message)
        {
            controller.TempData["StatusError"] = message;
        }



        /// <summary>設定 `TempData["StatusSuccess"]` 狀態訊息。</summary>
        /// <param name="page">目前 PageModel。</param>
        /// <param name="message">要顯示的成功訊息。</param>
        public static void SetStatusSuccess(this PageModel page, string message)
        {
            page.TempData["StatusSuccess"] = message;
        }

        /// <summary>設定 `TempData["StatusError"]` 狀態訊息。</summary>
        /// <param name="page">目前 PageModel。</param>
        /// <param name="message">要顯示的錯誤訊息。</param>
        public static void SetStatusError(this PageModel page, string message)
        {
            page.TempData["StatusError"] = message;
        }



    }
}
