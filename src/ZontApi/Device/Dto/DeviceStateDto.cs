using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record DeviceStateDto
(
    [property: JsonPropertyName("name")] string StateName,
    [property: JsonPropertyName("translate")] StateTranslateDto Translate
);

internal record StateTranslateDto
(
    [property: JsonPropertyName("ru")] string RussianDescription,
    [property: JsonPropertyName("en")] string EnglishDescription
);
