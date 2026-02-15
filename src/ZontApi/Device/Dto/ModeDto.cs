using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ModeDto
(
    [property: JsonPropertyName("id")] int ModeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("applied")] List<int> AppliedCircuits,
    [property: JsonPropertyName("can_be_applied")] List<int> AvailableCircuits,
    [property: JsonPropertyName("icon")] string Icon,
    [property: JsonPropertyName("schedule")] ScheduleDto? Schedule
);

internal record ScheduleDto
(
    [property: JsonPropertyName("hour")] int Hour,
    [property: JsonPropertyName("minute")] int Minute
);
