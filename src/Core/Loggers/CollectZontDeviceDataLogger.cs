using Microsoft.Extensions.Logging;

namespace CottageSensorAggregator.Core.Loggers;

public class CollectZontDeviceDataLogger<T> : BaseLogger<T>
{
    public CollectZontDeviceDataLogger(ILogger<T> innerLogger) : base(innerLogger)
    {
    }

    public override string GetLogType() => LogType.CollectDeviceData;
}
