using Microsoft.Extensions.Logging;

namespace CottageSensorAggregator.Core.Loggers;

public class HealthCheckLogger<T> : BaseLogger<T>
{
    public HealthCheckLogger(ILogger<T> innerLogger) : base(innerLogger) { }

    public override string GetLogType() => LogType.HealthCheck;
}
