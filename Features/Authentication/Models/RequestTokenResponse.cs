using System.Text.Json.Serialization;

namespace NovassatMovies.Features.Authentication;

public class RequestTokenResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("expires_at")]
    public string ExpiresAt{ get; set; }

    [JsonPropertyName("request_token")]
    public string RequestToken { get; set; }
}
