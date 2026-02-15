using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record CarStateDto
(
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("autostart")] AutoStartStateDto Autostart,
    [property: JsonPropertyName("car_view")] CarViewDto CarView,
    [property: JsonPropertyName("door_front_left")] bool DoorFrontLeftOpen,
    [property: JsonPropertyName("door_front_right")] bool DoorFrontRightOpen,
    [property: JsonPropertyName("door_rear_left")] bool DoorRearLeftOpen,
    [property: JsonPropertyName("door_rear_right")] bool DoorRearRightOpen,
    [property: JsonPropertyName("engine_block")] bool EngineBlockEnabled,
    [property: JsonPropertyName("engine_on")] bool EngineRunning,
    [property: JsonPropertyName("guard")] GuardStateDto GuardState,
    [property: JsonPropertyName("hood")] bool HoodOpen,
    [property: JsonPropertyName("position")] CarPositionDto Position,
    [property: JsonPropertyName("power_source")] string PowerSource,
    [property: JsonPropertyName("siren")] bool SirenEnabled,
    [property: JsonPropertyName("state")] CarMainStateDto State,
    [property: JsonPropertyName("trunk")] bool TrunkOpen
);

internal record CarViewDto
(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("mirrored")] bool IsMirrored,
    [property: JsonPropertyName("hidden_thermometers")] bool HideThermometers,
    [property: JsonPropertyName("color")] CarViewColorDto Color
);

internal record GuardStateDto
(
    [property: JsonPropertyName("state")] GuardState State,
    [property: JsonPropertyName("alarm")] bool IsAlarm
);

internal record CarViewColorDto
(
    [property: JsonPropertyName("glass")] string GlassColor,
    [property: JsonPropertyName("body")] string BodyColor,
    [property: JsonPropertyName("font")] string FontColor
);

internal record CarPositionDto
(
    [property: JsonPropertyName("time_str")] string TimeString,
    [property: JsonPropertyName("time")] long Time,
    [property: JsonPropertyName("x")] double XCoordinate,
    [property: JsonPropertyName("y")] double YCoordinate
);

internal record CarMainStateDto
(
    [property: JsonPropertyName("ru")] string RussianDescription,
    [property: JsonPropertyName("en")] string EnglishDescription
);

internal record AutoStartStateDto
(
    [property: JsonPropertyName("available")] bool IsAvailable,
    [property: JsonPropertyName("available_webasto")] bool IsWebastoAvailable,
    [property: JsonPropertyName("status")] AutoStartStatus Status,
    [property: JsonPropertyName("until")] long? Until // Unix timestamp
);
internal enum AutoStartStatus
{
    Disabled,
    Enabling,
    Enabled,
    Webasto,
    WebastoConfirmed
}
