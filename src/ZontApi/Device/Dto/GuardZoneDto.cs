using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record GuardZoneDto
(
    [property: JsonPropertyName("id")] int ZoneId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("state")] GuardState State,
    [property: JsonPropertyName("alarm")] bool IsAlarm
);
