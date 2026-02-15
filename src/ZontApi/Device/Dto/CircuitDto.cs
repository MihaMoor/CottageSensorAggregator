using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record CircuitDto
(
    [property: JsonPropertyName("id")] int CircuitId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("actual_temp")] double? ActualTemperature,
    [property: JsonPropertyName("target_temp")] double? TargetTemperature,
    [property: JsonPropertyName("current_mode")] int? CurrentMode,
    [property: JsonPropertyName("active")] bool IsActive,
    [property: JsonPropertyName("is_off")] bool IsOff,
    [property: JsonPropertyName("in_summer_mode")] bool IsSummerMode,
    [property: JsonPropertyName("min")] double? MinTemperature,
    [property: JsonPropertyName("max")] double? MaxTemperature,
    [property: JsonPropertyName("step")] double? TemperatureStep,
    [property: JsonPropertyName("error")] CircuitErrorDto? Error,
    [property: JsonPropertyName("icon")] string Icon,
    [property: JsonPropertyName("time_left")] int? TimeLeft
);

internal record CircuitErrorDto
(
    [property: JsonPropertyName("oem")] string OEMCode,
    [property: JsonPropertyName("text")] string ErrorText
);
