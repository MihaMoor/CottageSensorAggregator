using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record CircuitDto
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("actual_temp")] double ActualTemp,
    [property: JsonPropertyName("target_temp")] double TargetTemp,
    [property: JsonPropertyName("active")] bool IsActive,
    [property: JsonPropertyName("is_off")] bool IsOff,
    [property: JsonPropertyName("in_summer_mode")] bool IsSummerMode,
    [property: JsonPropertyName("min")] double MinTemp,
    [property: JsonPropertyName("max")] double MaxTemp,
    [property: JsonPropertyName("step")] double Step
);
