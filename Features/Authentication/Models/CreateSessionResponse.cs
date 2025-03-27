

namespace NovassatMovies.Features.Authentication;

public class CreateSessionResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("session_id")]
    public string SessionId { get; set; }
}