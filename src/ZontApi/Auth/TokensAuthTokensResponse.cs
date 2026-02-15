using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Auth;

internal record TokensAuthTokensResponse(
    [property: JsonPropertyName("token_id")]
    string TokenId,
    [property: JsonPropertyName("created")]
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    DateTime Created,
    [property: JsonPropertyName("last_used")]
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    DateTime LastUsed,
    [property: JsonPropertyName("client_name")]
    string ClientName,
    [property: JsonPropertyName("cookie")]
    string? Cookie,
    [property: JsonPropertyName("expires_at")]
    [property: JsonConverter(typeof(UnixNullableDateTimeConverter))]
    DateTime? ExpiresAt);
