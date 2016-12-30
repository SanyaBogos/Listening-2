using System;
using Microsoft.AspNetCore.Mvc.Filters;
using listening.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace listening.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private const string InternalServerError = "Internal Server Error";
        private Type[] _allowedToShowExceptions = new Type[] { typeof(FileUploadException), typeof(AuthenticationException) };

        public override void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;

            if (_allowedToShowExceptions.Contains(context.Exception.GetType()))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.WriteAsync(JsonConvert.SerializeObject(context.Exception.Message)).GetAwaiter().GetResult();
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.WriteAsync(JsonConvert.SerializeObject(InternalServerError)).GetAwaiter().GetResult();
            }
        }
    }
}
