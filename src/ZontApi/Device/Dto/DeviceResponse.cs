using CottageSensorAggregator.ZontApi.Device.Dto;
using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device;

internal record DeviceResponse(
    [property: JsonPropertyName("ok")] bool IsOk,
    [property: JsonPropertyName("devices")] List<DeviceDto> Devices
    );
