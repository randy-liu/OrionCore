using System;
using Microsoft.AspNetCore.Http;

namespace Orion.Mvc.Extensions
{
    /// <summary></summary>
    public static class RequestExtensions
    {

        /// <summary></summary>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (request.Headers != null) { return request.Headers["X-Requested-With"] == "XMLHttpRequest"; }

            return false;
        }


        /// <summary>Method == "GET"</summary>
        public static bool IsGetMethod(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            return "GET".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>Method == "POST"</summary>
        public static bool IsPostMethod(this HttpRequest request)
        {
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            return "POST".Equals(request.Method, StringComparison.OrdinalIgnoreCase);
        }


    }
}
