using Microsoft.AspNetCore.Builder;
using MicroTaskAPI.API.Middleware;

namespace MicroTaskAPI.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}