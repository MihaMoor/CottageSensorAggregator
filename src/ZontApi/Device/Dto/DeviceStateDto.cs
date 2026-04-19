using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

public record DeviceStateDto
(
    [property: JsonPropertyName("name")] string StateName,
    [property: JsonPropertyName("translate")] StateTranslateDto Translate
);

public record StateTranslateDto
(
    [property: JsonPropertyName("ru")] string RussianDescription,
    [property: JsonPropertyName("en")] string EnglishDescription
);
