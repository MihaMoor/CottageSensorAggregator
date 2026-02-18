using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Device.Dto;

internal record ConnectionInfoDto
(
    [property: JsonPropertyName("conn_state")] ConnectionStateDto? ConnectionState,
    [property: JsonPropertyName("gsm")] GSMConnectionDto? GSM
);

public record GSMConnectionDto
(
    [property: JsonPropertyName("connected")] bool Connected,
    [property: JsonPropertyName("imei")] string IMEI,
    [property: JsonPropertyName("operator")] string Operator,
    [property: JsonPropertyName("level")] int SignalLevel,
    [property: JsonPropertyName("sim")] SimInfoDto? SIM
);

public record ConnectionStateDto
(
    [property: JsonPropertyName("connected_to_server")]
    bool ConnectedToServer,
    [property: JsonPropertyName("connection_channel")] string? ConnectionChannel
);

public record SimInfoDto
(
    [property: JsonPropertyName("msisdn")] string PhoneNumber,
    [property: JsonPropertyName("iccid")] string ICCID,
    [property: JsonPropertyName("balance")] string Balance
);
