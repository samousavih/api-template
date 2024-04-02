using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace SampleOrg.WebAPI.Common.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (TaskCanceledException e)
        {
            await httpContext.Response.WriteAsync(Problems.TaskCancelled());
        }
        catch (Exception e)
        {
            _logger.Error(e, "Unhandled exception");
            await httpContext.Response.WriteAsync(Problems.InternalServerError());
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}