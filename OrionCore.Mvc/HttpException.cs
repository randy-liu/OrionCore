using System;
using System.Net;

namespace Orion.Mvc
{
    /// <summary></summary>
    public class HttpException : Exception
    {
        /// <summary></summary>
        public int StatusCode { get; private set; }


        /// <summary></summary>
        public HttpException(int statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary></summary>
        public HttpException(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        /// <summary></summary>
        public HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary></summary>
        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
        }

        /// <summary></summary>
        public HttpException(int statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        /// <summary></summary>
        public HttpException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = (int)statusCode;
        }

    }
}
