namespace CottageSensorAggregator.Api.Middlewares;

public class HealthCheckLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HealthCheckLoggingMiddleware> _logger;

    public HealthCheckLoggingMiddleware(RequestDelegate next, ILogger<HealthCheckLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await _next(context);
            stopwatch.Stop();

            _logger.LogInformation(
                "HealthCheck request completed in {Duration}ms. Status: {StatusCode}",
                stopwatch.ElapsedMilliseconds,
                context.Response.StatusCode);
        }
        else
        {
            await _next(context);
        }
    }
}

