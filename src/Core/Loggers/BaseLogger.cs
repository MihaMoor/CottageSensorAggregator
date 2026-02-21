using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace CottageSensorAggregator.Core.Loggers;

public abstract class BaseLogger<T> : ILogger<T>
{
    private readonly ILogger<T> _innerLogger;

    public BaseLogger(ILogger<T> innerLogger)
    {
        _innerLogger = innerLogger;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        => _innerLogger.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => _innerLogger.IsEnabled(logLevel);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        using (LogContext.PushProperty("LogType", GetLogType()))
        {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }
    }

    public abstract string GetLogType();
}
