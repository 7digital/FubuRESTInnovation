using System;
using System.Net;

namespace FubuRESTInnovation.Infrastructure.Errors
{
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string message)
        {
            Status = statusCode;
        }

        public HttpStatusCode Status { get; set; }
    }
}