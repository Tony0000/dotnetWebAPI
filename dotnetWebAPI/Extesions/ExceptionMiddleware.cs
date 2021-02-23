using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPI.ActionResult.Model;

namespace WebAPI.Extesions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;
            var type = exception.GetType().Name;
            var detail = "An internal error occurred.";
            ValidationError validationError = null;

            switch (exception)
            {
                case InvalidCastException _:
                case InvalidEnumArgumentException _:
                    status = HttpStatusCode.BadRequest;
                    detail = exception.Message;
                    break;
                case NullReferenceException _:
                    status = HttpStatusCode.NotFound;
                    detail = "Object not found";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            string result;

            if (validationError is null)
                result = JsonConvert.SerializeObject(new {status, type, detail});
            else
                result = JsonConvert.SerializeObject(new {status, type, Errors = validationError});

            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}