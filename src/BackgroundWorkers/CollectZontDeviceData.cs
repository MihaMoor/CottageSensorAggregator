using CottageSensorAggregator.Core.Loggers;
using CottageSensorAggregator.ZontApi;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CottageSensorAggregator.BackgroundWorkers;

public class CollectZontDeviceData : BackgroundService
{
    private readonly ZontRepository _zontRepository;
    private readonly ZontSettings _zontSettings;
    private readonly CollectZontDeviceDataLogger<CollectZontDeviceData> _logger;

    public CollectZontDeviceData(
        ZontRepository zontRepository,
        IOptions<ZontSettings> zontSettings,
        CollectZontDeviceDataLogger<CollectZontDeviceData> logger)
    {
        _zontRepository = zontRepository;
        _zontSettings = zontSettings.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                string devices = await _zontRepository.GetDevicesAsync(cancellationToken);

                _logger.LogInformation("Получены данные по устройствам");
                _logger.LogInformation(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
            }

            await Task.Delay(_zontSettings.CollectDeviceDataInterval);
        }
    }
}
