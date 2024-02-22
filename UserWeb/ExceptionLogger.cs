using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserWeb
{
    public class ExceptionLogger
    {
        private readonly ILogger<ExceptionLogger> logger;

        public ExceptionLogger(ILogger<ExceptionLogger> _logger)
        {
            logger = _logger;
        }

        public async Task Log(HttpContext context, Exception ex)
        {
            var route = context.Request.Path.Value; 
            if (context.Request.Body != null)
                context.Request.Body.Seek(0, SeekOrigin.Begin);

            string requestBody = context.Request.Body == null ? "" : await GetRequestBody(context, route);

            var headers = JsonConvert.SerializeObject(context.Request.Headers, Formatting.Indented);
            var queryString = context.Request.QueryString.Value;

            var logInformation = $"Time : {DateTime.Now}\n" +
                $"Route : {route}\n" +
                $"QueryParams : {queryString}\n" +
                $"Request body : {requestBody}\n" +
                $"Headers : {headers}\n" +
                $"Exception Message : {ex?.Message}\n";

            if (ex is not UserValidationException)
            {
                logInformation +=
                    $"Inner Exception Message : {ex?.InnerException}\n" +
                    $"Stack trace : {ex?.StackTrace}\n";
            }
            logger.LogInformation(logInformation);
        }

        public static async Task<string> GetRequestBody(HttpContext context, string route)
        {
            string requestBody = "";
            using (var reader = new StreamReader(context.Request.Body))
                requestBody = await reader.ReadToEndAsync();
            return requestBody;
        }
    }
}
