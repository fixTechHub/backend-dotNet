using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WebApiDotNet.Services;

namespace WebApiDotNet.Middleware
{
    public class ActionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ActionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            try
            {
                // Capture request details
                var requestBody = await GetRequestBodyAsync(context.Request);
                var requestQuery = context.Request.QueryString.ToString();
                var method = context.Request.Method;
                var route = context.Request.Path;
                var ip = context.Connection.RemoteIpAddress?.ToString();
                var userAgent = context.Request.Headers["User-Agent"].ToString();

                // Create a new response body stream to capture the response
                using var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                // Call the next middleware
                await _next(context);

                // Capture response details
                var statusCode = context.Response.StatusCode;
                var responseBody = await GetResponseBodyAsync(responseBodyStream);

                // Copy the response back to the original stream
                responseBodyStream.Position = 0;
                await responseBodyStream.CopyToAsync(originalBodyStream);

                // Log the action (async without awaiting to avoid blocking)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // Extract user ID from context if available (you might need to adjust this based on your authentication setup)
                        var userId = context.User?.Identity?.IsAuthenticated == true 
                            ? context.User.FindFirst("sub")?.Value ?? "anonymous"
                            : "anonymous";

                        var actionType = DetermineActionType(method, route);
                        var description = GenerateDescription(method, route, statusCode);

                        // Validate and sanitize data
                        var sanitizedData = new
                        {
                            UserId = string.IsNullOrEmpty(userId) ? "anonymous" : userId,
                            ActionType = string.IsNullOrEmpty(actionType) ? "UNKNOWN" : actionType,
                            Method = string.IsNullOrEmpty(method) ? "UNKNOWN" : method,
                            Route = string.IsNullOrEmpty(route) ? "/" : route,
                            Parameters = (string)null, // You can extract route parameters if needed
                            Query = string.IsNullOrEmpty(requestQuery) ? "" : requestQuery,
                            Body = string.IsNullOrEmpty(requestBody) ? "" : requestBody,
                            StatusCode = statusCode,
                            Ip = string.IsNullOrEmpty(ip) ? "unknown" : ip,
                            UserAgent = string.IsNullOrEmpty(userAgent) ? "unknown" : userAgent,
                            Description = string.IsNullOrEmpty(description) ? $"{method} {route}" : description
                        };

                        // Create a new scope to avoid IFeatureCollection disposed error
                        var scopeFactory = context.RequestServices.GetRequiredService<IServiceScopeFactory>();
                        using var scope = scopeFactory.CreateScope();
                        var actionLogService = scope.ServiceProvider.GetService<ActionLogService>();
                        
                        if (actionLogService != null)
                        {
                            await actionLogService.LogActionAsync(
                                userId: sanitizedData.UserId,
                                actionType: sanitizedData.ActionType,
                                method: sanitizedData.Method,
                                route: sanitizedData.Route,
                                parameters: sanitizedData.Parameters,
                                query: sanitizedData.Query,
                                body: sanitizedData.Body,
                                statusCode: sanitizedData.StatusCode,
                                ip: sanitizedData.Ip,
                                userAgent: sanitizedData.UserAgent,
                                description: sanitizedData.Description
                            );
                        }
                        else
                        {
                            Console.WriteLine("Warning: ActionLogService is null, skipping action logging");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the error but don't throw to avoid breaking the request
                        Console.WriteLine($"Error logging action: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    }
                });
            }
            catch (Exception ex)
            {
                // Restore the original body stream
                context.Response.Body = originalBodyStream;
                throw;
            }
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            if (request.Body == null) return string.Empty;

            request.EnableBuffering();
            request.Body.Position = 0;

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;

            return body;
        }

        private async Task<string> GetResponseBodyAsync(Stream responseBodyStream)
        {
            responseBodyStream.Position = 0;
            using var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true);
            return await reader.ReadToEndAsync();
        }

        private string DetermineActionType(string method, string route)
        {
            return method.ToUpper() switch
            {
                "GET" => "READ",
                "POST" => "CREATE",
                "PUT" => "UPDATE",
                "PATCH" => "UPDATE",
                "DELETE" => "DELETE",
                _ => "UNKNOWN"
            };
        }

        private string GenerateDescription(string method, string route, int statusCode)
        {
            var statusText = statusCode switch
            {
                200 => "Success",
                201 => "Created",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };

            return $"{method} {route} - {statusText} ({statusCode})";
        }
    }

    // Extension method to register the middleware
    public static class ActionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseActionLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActionLoggingMiddleware>();
        }
    }
} 