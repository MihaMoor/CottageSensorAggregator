using CottageSensorAggregator.ZontApi.Device.Dto;
using PostgresDb.ZontEntities;

namespace Mappers;

public static class ZontApiToPostgresMapper
{
    public static ZontDeviceEntity ToZontDeviceEntity(this DeviceDto deviceDto)
    {
        var entity = new ZontDeviceEntity
        {
            DeviceId = deviceDto.DeviceInfo.DeviceTypeId,
            DeviceModel = deviceDto.DeviceInfo.Model,
            IsOnline = deviceDto.IsOnline,
            Name = deviceDto.Name,
            ZontId = deviceDto.Id,
            HardwareVersion = deviceDto.DeviceInfo.Version.HardwareVersion,
            SoftwareVersion = deviceDto.DeviceInfo.Version.SoftwareVersion,
            FetchedAt = DateTime.UtcNow,
        };

        return entity;
    }

    public static ZontCircuitsEntity ToZontCircuitsEntity(this CircuitDto circuitDto)
    {
        var entity = new ZontCircuitsEntity
        {
            ZontId = circuitDto.Id,
            FetchedAt = DateTime.UtcNow,
            IsActive = circuitDto.IsActive,
            Name = circuitDto.Name,
            ActualTemp = circuitDto.ActualTemp,
            CurrentTemp = circuitDto.TargetTemp,
            Max = (int)circuitDto.MaxTemp,
            Min = (int)circuitDto.MinTemp,
            Status = circuitDto.Status,
            Step = (int)circuitDto.Step
        };

        return entity;
    }

    public static ZontSensoreEntity ToZontSensoreEntity(this SensorDto sensorDto)
    {
        var entity = new ZontSensoreEntity
        {
            FetchedAt = DateTime.UtcNow,
            ZontId = sensorDto.Id,
            Name = sensorDto.Name,
            Status = sensorDto.Status,
            Type = sensorDto.Type,
            Unit = sensorDto.Unit,
            Value = sensorDto.Value,
        };

        return entity;
    }
}
