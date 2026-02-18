using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record SensorDto
(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("value")] double Value,
    [property: JsonPropertyName("unit")] string Unit
);
