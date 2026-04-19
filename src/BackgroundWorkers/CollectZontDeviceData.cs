using CottageSensorAggregator.Core.Loggers;
using CottageSensorAggregator.ZontApi;
using CottageSensorAggregator.ZontApi.Device;
using Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PostgresDb;

namespace CottageSensorAggregator.BackgroundWorkers;

public class CollectZontDeviceData : BackgroundService
{
    private readonly ZontRepository _zontRepository;
    private readonly ZontSettings _zontSettings;
    private readonly ApplicationLogger<CollectZontDeviceData> _logger;
    private readonly PostgresContext _context;
    private readonly IServiceScopeFactory _scopeFactory;

    public CollectZontDeviceData(
        IServiceScopeFactory scopeFactory,
        ZontRepository zontRepository,
        IOptions<ZontSettings> zontSettings,
        ApplicationLogger<CollectZontDeviceData> logger)
    {
        _scopeFactory = scopeFactory;
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
                _logger.LogInformation("Получены данные по устройствам");
                DeviceResponse devices = await _zontRepository.GetDevicesAsync(cancellationToken);

                using var scope = _scopeFactory.CreateScope();
                using PostgresContext postgresContext = scope.ServiceProvider.GetService<PostgresContext>()!;

                await postgresContext.AddRangeAsync(devices.Devices.Select(x => x.ToZontDeviceEntity()), cancellationToken);
                await postgresContext.AddRangeAsync(devices.Devices.SelectMany(
                    x => x.Circuits.Select(
                        y => y.ToZontCircuitsEntity())),
                    cancellationToken);
                await postgresContext.AddRangeAsync(devices.Devices.SelectMany(
                    x => x.Sensors.Select(
                        y => y.ToZontSensoreEntity())),
                        cancellationToken);

                await postgresContext.SaveChangesAsync();

                _logger.LogInformation("Данные по устройствам сохранены в БД");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.StackTrace);
            }

            await Task.Delay(_zontSettings.CollectDeviceDataInterval);
        }
    }
}
