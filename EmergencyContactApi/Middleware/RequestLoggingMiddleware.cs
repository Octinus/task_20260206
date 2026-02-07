using EmergencyContactApi.Models.Logging;
using System.Text;

namespace EmergencyContactApi.Middleware
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {

            RequestLog requestLog = await GetRequestInfoAsync(httpContext);

            httpContext.Items["RequestLog"] = requestLog;

            await next(httpContext);
        }

        private async Task<RequestLog> GetRequestInfoAsync(HttpContext httpContext)
        {
            RequestLog requestLog = new RequestLog();

            requestLog.RequestHost = httpContext.Request.Host.Host;
            requestLog.ClientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            requestLog.HttpMethod = httpContext.Request.Method;
            requestLog.ContentType = httpContext.Request.ContentType ?? string.Empty;

            if (httpContext.Request.Method == HttpMethods.Get)
            {
                var request = httpContext.Request;

                var path = request.Path.HasValue ? request.Path.Value : string.Empty;

                var query = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;

                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(query))
                {
                    requestLog.UrlParameters = $"{path}{query}";
                }
                else if (!string.IsNullOrEmpty(path))
                {
                    requestLog.UrlParameters = path;
                }
                else if (!string.IsNullOrEmpty(query))
                {
                    requestLog.UrlParameters = query;
                }
            }
            else
            {
                httpContext.Request.EnableBuffering();
                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    string body = await reader.ReadToEndAsync();

                    if (string.IsNullOrEmpty(body))
                        requestLog.RequestBody = string.Empty;
                    else
                        requestLog.RequestBody = body;
                }
                httpContext.Request.Body.Position = 0;
            }

            return requestLog;
        }
    }
}
