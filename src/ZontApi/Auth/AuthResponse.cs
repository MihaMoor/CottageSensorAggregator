using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Auth;

internal record AuthResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("token_id")] string TokenId);
