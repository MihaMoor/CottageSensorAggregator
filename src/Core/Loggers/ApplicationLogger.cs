using Microsoft.Extensions.Logging;

namespace CottageSensorAggregator.Core.Loggers;

public class ApplicationLogger<T> : BaseLogger<T>
{
    public ApplicationLogger(ILogger<T> innerLogger) : base(innerLogger) { }

    public override string GetLogType() => LogType.Application;
}
