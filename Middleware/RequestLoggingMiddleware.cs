using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TodoAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var requestPath = context.Request.Path;
            var requestTime = DateTime.UtcNow;

            Console.WriteLine($"[{requestTime}] Request from {ipAddress} to {requestPath}");
            await LogToFile(ipAddress, requestPath, requestTime);

            await _next(context);
        }

        private async Task LogToFile(string? ip, string path, DateTime time)
        {
            var logMessage = $"{time:yyyy-MM-dd HH:mm:ss} | IP: {ip} | Path: {path}";
            var logFilePath = "request_logs.txt";

            await File.AppendAllTextAsync(logFilePath, logMessage + Environment.NewLine);
        }
    }
}
