using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    private readonly IServiceScopeFactory _scopedFactory;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, IServiceScopeFactory scopedFactory)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scopedFactory = scopedFactory ?? throw new ArgumentNullException(nameof(scopedFactory));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context == null) 
            throw new ArgumentNullException(nameof(context));

        var method = context.Request.Method;
        var path = context.Request.Path + context.Request.QueryString;

        var sw = Stopwatch.StartNew();
        
        try
        {
            _logger.LogDebug("Handling request {Method} {Path}", method, path);
            using(var scope = _scopedFactory.CreateScope())
            {
                //var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            }
            await _next(context);


            sw.Stop();
            var statusCode = context.Response.StatusCode;
            var elapsedTime = sw.ElapsedMilliseconds;
            _logger.LogInformation(
                "Finished handling request {Method} {Path} with status code {StatusCode} in {ElapsedTime} ms",
                method, path, statusCode, elapsedTime
            );
        }
        catch (Exception ex)
        {
            sw.Stop();
            var elapsedTime = sw.ElapsedMilliseconds;

            _logger.LogError(ex,
            "An error occurred while handling request {Method} {Path} after {ElapsedTime} ms",
            method, path, elapsedTime);
            throw;
        }
    }
}