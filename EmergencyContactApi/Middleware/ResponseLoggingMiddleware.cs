using EmergencyContactApi.Models.Logging;
using System.Text;
using System.Text.Json;

namespace EmergencyContactApi.Middleware
{
    public class ResponseLoggingMiddleware : IMiddleware
    {
        private readonly ILogger<ResponseLoggingMiddleware> _logger;

        public ResponseLoggingMiddleware(ILogger<ResponseLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var originalBody = httpContext.Response.Body;
            using var newBody = new MemoryStream();
            httpContext.Response.Body = newBody;
            await next(httpContext);

            string responseBody;
            {
                newBody.Seek(0, SeekOrigin.Begin);
                responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            }
            RequestLog requestLog = httpContext.Items["RequestLog"] as RequestLog;

            if (!string.IsNullOrWhiteSpace(responseBody))
            {
                ResponseLog responseLog = new ResponseLog { ResponseBody = responseBody };

                int status = httpContext.Response.StatusCode;

                if (status >= 200 && status < 300)
                {
                    _logger.LogInformation("Log Information: {@RequestInfo}, {@ResponseInfo}",requestLog, responseLog);
                }
                else if (status >= 400 && status < 500)
                {
                    _logger.LogWarning("Log Information: {@RequestInfo}, {@ResponseInfo}", requestLog, responseLog);
                }
                else if (status >= 500)
                {
                    _logger.LogError("Log Information: {@RequestInfo}, {@ResponseInfo}", requestLog, responseLog);
                }
                else
                {
                    _logger.LogInformation("Log Information: {@RequestInfo}, {@ResponseInfo}", requestLog, responseLog);
                }
            }

            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }
    }
}
