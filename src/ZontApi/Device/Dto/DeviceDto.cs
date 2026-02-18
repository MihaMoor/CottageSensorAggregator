using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record DeviceDto
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("online")] bool IsOnline,
    [property: JsonPropertyName("device_info")] DeviceInfoDto DeviceInfo,
    [property: JsonPropertyName("state")] DeviceStateDto State,
    [property: JsonPropertyName("connection")] ConnectionInfoDto Connection,
    [property: JsonPropertyName("sim_info")] SimInfoDto SimInfo,
    [property: JsonPropertyName("circuits")] List<CircuitDto> Circuits,
    [property: JsonPropertyName("modes")] List<ModeDto> Modes,
    [property: JsonPropertyName("sensors")] List<SensorDto> Sensors
);
