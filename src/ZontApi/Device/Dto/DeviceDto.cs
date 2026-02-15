using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record DeviceDto
(
    [property: JsonPropertyName("id")] int DeviceId,
    [property: JsonPropertyName("name")] string DeviceName,
    [property: JsonPropertyName("online")] bool IsOnline,
    [property: JsonPropertyName("device_info")] DeviceInfoDto DeviceInfo,
    [property: JsonPropertyName("state")] DeviceStateDto DeviceState,
    [property: JsonPropertyName("connection")] ConnectionInfoDto ConnectionInfo,
    [property: JsonPropertyName("sim_info")] SimInfoDto SimInfo,
    [property: JsonPropertyName("actuators")] ActuatorsDto Actuators,
    [property: JsonPropertyName("controls")] ControlsDto Controls,
    [property: JsonPropertyName("circuits")] List<CircuitDto> Circuits,
    [property: JsonPropertyName("guard_zones")] List<GuardZoneDto> GuardZones,
    [property: JsonPropertyName("modes")] List<ModeDto> Modes,
    [property: JsonPropertyName("sensors")] List<SensorDto> Sensors,
    [property: JsonPropertyName("scenarios")] List<ScenarioDto> Scenarios,
    [property: JsonPropertyName("car_state")] CarStateDto CarState
);
