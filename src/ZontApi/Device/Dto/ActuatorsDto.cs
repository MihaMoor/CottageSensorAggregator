using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ActuatorsDto
(
    [property: JsonPropertyName("adapters")] List<BoilerAdapterDto> Adapters,
    [property: JsonPropertyName("pumps")] List<PumpDto> Pumps,
    [property: JsonPropertyName("relays")] List<RelayDto> Relays,
    [property: JsonPropertyName("taps")] List<TapDto> Taps
);

internal record BoilerAdapterDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("no_connection")] bool NoConnection,
    [property: JsonPropertyName("failed")] bool Failed,
    [property: JsonPropertyName("unknown")] bool Unknown,
    [property: JsonPropertyName("error")] BoilerErrorReport? Error,
    [property: JsonPropertyName("status")] List<OpenThermValue>? Status,
    [property: JsonPropertyName("boiler_config")] List<OpenThermValue>? BoilerConfig,
    [property: JsonPropertyName("heating")] List<OpenThermValue>? Heating,
    [property: JsonPropertyName("dhw")] List<OpenThermValue>? DHW,
    [property: JsonPropertyName("other")] List<OpenThermValue>? Other
);

internal record TapDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("opened")] bool IsOpened,
    [property: JsonPropertyName("opening")] bool IsOpening,
    [property: JsonPropertyName("idle")] bool IsIdle,
    [property: JsonPropertyName("closing")] bool IsClosing,
    [property: JsonPropertyName("closed")] bool IsClosed,
    [property: JsonPropertyName("failed")] bool Failed,
    [property: JsonPropertyName("testing")] bool IsTesting,
    [property: JsonPropertyName("unknown")] bool Unknown
);

internal record RelayDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("on")] bool IsOn,
    [property: JsonPropertyName("failed")] bool Failed,
    [property: JsonPropertyName("testing")] bool IsTesting,
    [property: JsonPropertyName("unknown")] bool Unknown
);

internal record PumpDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("on")] bool IsOn,
    [property: JsonPropertyName("summer_mode")] bool IsSummerMode,
    [property: JsonPropertyName("testing")] bool IsTesting,
    [property: JsonPropertyName("unknown")] bool Unknown
);

internal record BoilerErrorReport(
    [property: JsonPropertyName("oem_code")] int OEMCode,
    [property: JsonPropertyName("oem_error")] string OEMError,
    [property: JsonPropertyName("description")] object? Description,
    [property: JsonPropertyName("flags")] List<string>? Flags,
    [property: JsonPropertyName("flag_descriptions")] List<string>? FlagDescriptions,
    [property: JsonPropertyName("display_lines")] List<string>? DisplayLines
);

internal record OpenThermValue(
    [property: JsonPropertyName("flag")] string Flag,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("display_value")] string DisplayValue,
    [property: JsonPropertyName("value")] object Value
);
