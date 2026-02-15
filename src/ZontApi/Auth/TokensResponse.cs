using System.Text.Json.Serialization;

namespace CottageSensorAggregator.ZontApi.Auth;

internal record TokensResponse(
    [property: JsonPropertyName("ok")] bool Ok,
    [property: JsonPropertyName("auth_tokens")] TokensAuthTokensResponse[] AuthTokens);
