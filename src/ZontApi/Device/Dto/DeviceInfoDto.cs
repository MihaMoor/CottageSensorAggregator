using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record DeviceInfoDto
(
    [property: JsonPropertyName("id")] string DeviceTypeId,
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("serial")] string SerialNumber,
    [property: JsonPropertyName("widget_type")] string WidgetType,
    [property: JsonPropertyName("version")] VersionInfoDto Version
);

internal record VersionInfoDto
(
    [property: JsonPropertyName("hardware")] string HardwareVersion,
    [property: JsonPropertyName("software")] string SoftwareVersion
);
