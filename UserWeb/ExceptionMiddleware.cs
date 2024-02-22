using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserWeb
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, ExceptionLogger exceptionLogger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await exceptionLogger.Log(httpContext, ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";


            if (exception is UserValidationException)
            {
                context.Response.StatusCode = 400;
                var errorResponse = new ErrorResponse { ErrorText = exception.Message };
                await context.Response.WriteAsync(errorResponse.ToString());
            }
            else
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Произошла ошибка. Обратитесь в службу поддержки");
            }
        }


        private class ErrorResponse
        {
            public string ErrorText { get; set; }

            public override string ToString()
            {
                return JsonSerializer.Serialize(this);
            }
        }
    }

    public static class ExceptionMiddlewareExtension
    {
        public static void UseUserExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
