using Hng_Stage1_BackendTrack.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.API.Middlewares
{
        public class ExceptionMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionMiddleware> _logger;

            public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (ApiException ex)
                {
                    _logger.LogWarning(ex, "Handled API exception: {Message}", ex.Message);
                    await WriteProblemDetailsAsync(context, ex);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception");
                    var apiEx = new InternalServerErrorException(ex.Message);
                    await WriteProblemDetailsAsync(context, apiEx);
                }
            }

            private static async Task WriteProblemDetailsAsync(HttpContext context, ApiException ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = ex.StatusCode,
                    Title = ex.ErrorCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };

                var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.StatusCode;

                await context.Response.WriteAsync(json);
            }
        
    }

}
