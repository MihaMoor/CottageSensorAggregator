using CottageSensorAggregator.Core;
using CottageSensorAggregator.Core.Loggers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CottageSensorAggregator.BackgroundWorkers;

public class HealthCheckReporter : BackgroundService
{
    private readonly TimeSpan _interval;

    private readonly HealthCheckLogger<HealthCheckReporter> _logger;
    private readonly HealthCheckService _healthCheckService;
    private readonly HealthCheckSettings _healthCheckSettings;

    public HealthCheckReporter(
        HealthCheckService healthCheckService,
        IOptions<HealthCheckSettings> healthCheckSettings,
        HealthCheckLogger<HealthCheckReporter> logger)
    {
        _healthCheckSettings = healthCheckSettings.Value;
        _interval = _healthCheckSettings.Interval;
        _logger = logger;
        _healthCheckService = healthCheckService;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Выполняем health‑check
                var report = await _healthCheckService.CheckHealthAsync(cancellationToken);

                // Логируем результат в Elasticsearch через Serilog
                LogHealthStatus(report);

                // Ждём перед следующей проверкой
                await Task.Delay(_interval, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
            }
        }
    }

    private void LogHealthStatus(HealthReport report)
    {
        var healthData = new
        {
            Timestamp = DateTime.UtcNow,
            OverallStatus = report.Status.ToString(),
            TotalDurationMs = report.TotalDuration.TotalMilliseconds,
            Checks = report.Entries.Select(e => new
            {
                Name = e.Key,
                Status = e.Value.Status.ToString(),
                Description = e.Value.Description,
                DurationMs = e.Value.Duration.TotalMilliseconds
            }).ToList()
        };

        _logger.LogInformation("Service health status: {@HealthData}", healthData);
    }

}

