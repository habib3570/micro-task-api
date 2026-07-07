using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MicroTaskAPI.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace MicroTaskAPI.API.Middleware
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                DuplicateEntityException => HttpStatusCode.Conflict,
                InsufficientCoinException => HttpStatusCode.BadRequest,
                InvalidSubmissionStatusException => HttpStatusCode.BadRequest,
                InvalidWithdrawalException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                success = false,
                message = exception.Message,
                statusCode = (int)statusCode
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}