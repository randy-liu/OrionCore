using System;
using Microsoft.AspNetCore.Http;

namespace Orion.Mvc.Extensions
{
    /// <summary>提供 `HttpRequest` 常用判斷擴充方法。</summary>
    public static class RequestExtensions
    {

        /// <summary>判斷是否為 Ajax 請求（`X-Requested-With=XMLHttpRequest`）。</summary>
        /// <param name="request">目前 HTTP 請求。</param>
        /// <returns>是 Ajax 請求時回傳 `true`。</returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Headers != null) { return request.Headers["X-Requested-With"] == "XMLHttpRequest"; }

            return false;
        }


        /// <summary>判斷 HTTP 方法是否為 `GET`。</summary>
        /// <param name="request">目前 HTTP 請求。</param>
        /// <returns>方法為 `GET` 時回傳 `true`。</returns>
        public static bool IsGetMethod(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            return "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>判斷 HTTP 方法是否為 `POST`。</summary>
        /// <param name="request">目前 HTTP 請求。</param>
        /// <returns>方法為 `POST` 時回傳 `true`。</returns>
        public static bool IsPostMethod(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            return "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
        }


    }
}
