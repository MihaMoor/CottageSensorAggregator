using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ControlsDto
(
    [property: JsonPropertyName("buttons")] List<ButtonDto> Buttons,
    [property: JsonPropertyName("regulators")] List<RegulatorDto> Regulators,
    [property: JsonPropertyName("statuses")] List<StatusDto> Statuses,
    [property: JsonPropertyName("toggle_buttons")] List<ToggleButtonDto> ToggleButtons
);

internal record ButtonDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("view")] string? ViewType,
    [property: JsonPropertyName("icon")] string Icon
);

internal record ToggleButtonDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] StatusLabelDto Name,
    [property: JsonPropertyName("active")] bool? IsActive,
    [property: JsonPropertyName("view")] string? ViewType,
    [property: JsonPropertyName("icon")] string Icon
);

internal record StatusDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] StatusLabelDto Name,
    [property: JsonPropertyName("active")] bool? IsActive,
    [property: JsonPropertyName("view")] string? ViewType,
    [property: JsonPropertyName("icon")] string Icon
);

internal record RegulatorDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] double Value,
    [property: JsonPropertyName("min")] double MinValue,
    [property: JsonPropertyName("max")] double MaxValue,
    [property: JsonPropertyName("step")] double Step,
    [property: JsonPropertyName("unit")] string Unit,
    [property: JsonPropertyName("view")] string? ViewType,
    [property: JsonPropertyName("icon")] string Icon
);

internal record StatusLabelDto(
    [property: JsonPropertyName("name")] string LabelName,
    [property: JsonPropertyName("active_label")] string ActiveLabel,
    [property: JsonPropertyName("inactive_label")] string InactiveLabel
);
