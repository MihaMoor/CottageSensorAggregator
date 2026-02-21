namespace CottageSensorAggregator.Core;

public record HealthCheckSettings(TimeSpan Interval, bool IsEnabled, TimeSpan Delay, TimeSpan Period)
{
    public HealthCheckSettings()
        : this(
              TimeSpan.FromSeconds(5),
              true,
              TimeSpan.FromSeconds(1),
              TimeSpan.FromSeconds(1))
    { }
}
