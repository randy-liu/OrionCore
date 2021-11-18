using System;
using System.Net;

namespace Orion.Mvc
{
    public class HttpException : Exception
    {
        public int StatusCode { get; private set; }


        public HttpException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
        }

        public HttpException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = (int)statusCode;
        }

        public HttpException(int statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = (int)statusCode;
        }

    }
}
