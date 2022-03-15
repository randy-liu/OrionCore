using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Orion.Mvc.Extensions
{
    /// <summary></summary>
    public static class ControllerExtensions
    {

        /// <summary></summary>
        public static void SetStatusSuccess(this Controller controller, string message)
        {
            controller.TempData["StatusSuccess"] = message;
        }


        /// <summary></summary>
        public static void SetStatusError(this Controller controller, string message)
        {
            controller.TempData["StatusError"] = message;
        }



        /// <summary></summary>
        public static void SetStatusSuccess(this PageModel page, string message)
        {
            page.TempData["StatusSuccess"] = message;
        }

        /// <summary></summary>
        public static void SetStatusError(this PageModel page, string message)
        {
            page.TempData["StatusError"] = message;
        }



    }
}