using System.Net;
using System.Text.Json;
using SmartBudgetAPI.Application.DTOs.Common;

namespace SmartBudgetAPI.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            
            var statusCode = ex switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            List<string>? errors = null;
            if (ex is FluentValidation.ValidationException valEx)
            {
                errors = valEx.Errors.Select(e => e.ErrorMessage).ToList();
            }
            else if (_env.IsDevelopment())
            {
                errors = new List<string> { ex.StackTrace ?? "" };
            }

            var response = ApiResponse<object>.FailureResponse(
                _env.IsDevelopment() || ex is FluentValidation.ValidationException ? ex.Message : "An unexpected error occurred.",
                errors
            );

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
