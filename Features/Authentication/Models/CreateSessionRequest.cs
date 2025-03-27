namespace NovassatMovies.Features.Authentication;

public class CreateSessionRequest
{
    [JsonPropertyName("request_token")]
    public string RequesToken { get; set; }
}
