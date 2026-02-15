using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ConnectionInfoDto
(
    [property: JsonPropertyName("gsm")] GSMConnectionDto? GSM,
    [property: JsonPropertyName("wifi")] WiFiConnectionDto? WiFi,
    [property: JsonPropertyName("ethernet")] EthernetConnectionDto? Ethernet,
    [property: JsonPropertyName("conn_state")] ConnectionStateDto? ConnectionState
);

public record GSMConnectionDto
(
    [property: JsonPropertyName("connected")] bool Connected,
    [property: JsonPropertyName("imei")] string? IMEI,
    [property: JsonPropertyName("operator")] string? Operator,
    [property: JsonPropertyName("level")] int? SignalLevel,
    [property: JsonPropertyName("sim")] SimInfoDto? SIM
);

public record WiFiConnectionDto
(
    [property: JsonPropertyName("connected")] bool Connected,
    [property: JsonPropertyName("rssi")] int? RSSI,
    [property: JsonPropertyName("netname")] string? NetworkName,
    [property: JsonPropertyName("signal_strength")] string? SignalStrength,
    [property: JsonPropertyName("ip")] string? IPAddress
);

public record EthernetConnectionDto
(
    [property: JsonPropertyName("connected")]
    bool Connected,
    [property: JsonPropertyName("ip")] string? IPAddress,
    [property: JsonPropertyName("mac")] string? MACAddress,
    [property: JsonPropertyName("mask")] string? SubnetMask,
    [property: JsonPropertyName("gateway")] string? Gateway
);

public record ConnectionStateDto
(
    [property: JsonPropertyName("connected_to_server")]
    bool ConnectedToServer,
    [property: JsonPropertyName("connection_channel")] string? ConnectionChannel
);

public record SimInfoDto
(
    [property: JsonPropertyName("msisdn")]
    string? PhoneNumber,
    [property: JsonPropertyName("iccid")] string? ICCID,
    [property: JsonPropertyName("balance")] string? Balance
);
