using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ModeDto
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("applied")] List<int> AppliedCircuits,
    [property: JsonPropertyName("can_be_applied")] List<int> AvailableCircuits
);
