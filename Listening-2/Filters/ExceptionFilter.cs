using System;
using Microsoft.AspNetCore.Mvc.Filters;
using listening.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace listening.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        private const string InternalServerError = "Internal Server Error";
        private Type[] _allowedToShowExceptions = new Type[] {
            typeof(FileUploadException), typeof(AuthenticationException), typeof(TextException) };

        public override void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            string errorMessage;

            if (_allowedToShowExceptions.Contains(context.Exception.GetType()))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = context.Exception.Message;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorMessage = InternalServerError;
            }

            context.Result = new ObjectResult(new { message = errorMessage });
        }
    }
}
