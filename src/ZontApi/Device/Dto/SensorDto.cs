using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record SensorDto
(
    [property: JsonPropertyName("id")] int SensorId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] SensorType Type,
    [property: JsonPropertyName("status")] SensorStatus Status,
    [property: JsonPropertyName("value")] double? Value,
    [property: JsonPropertyName("triggered")] bool? IsTriggered,
    [property: JsonPropertyName("unit")] string Unit,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("icon")] string Icon,
    [property: JsonPropertyName("limits")] LimitsDto? Limits,
    [property: JsonPropertyName("battery")] int? BatteryLevel,
    [property: JsonPropertyName("rssi")] double? Rssi,
    [property: JsonPropertyName("signal_strength")] SignalStrength SignalStrength
);

internal enum SensorType
{
    Temperature,
    Voltage,
    Pressure,
    Humidity,
    Opening,
    Motion,
    Leakage,
    Smoke,
    RoomThermostat,
    BoilerFailure,
    PowerSource,
    Modulation,
    Discrete,
    DhwSpeed,
    Gas,
    Other
}

internal enum SensorStatus
{
    Unknown,
    Ok,
    Failure,
    Alarm,
    SilentAlarm
}

internal record LimitsDto
(
    [property: JsonPropertyName("high")] double? HighLimit,
    [property: JsonPropertyName("low")] double? LowLimit
);

internal enum SignalStrength
{
    NoSignal,
    Weak,
    Fair,
    Good,
    Excellent
}
