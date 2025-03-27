namespace NovassatMovies.Features.Movies;

public class AddOrRemoveResponse
{

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }

    [JsonPropertyName("status_message")]
    public string StatusMessage { get; set; }

    public bool WasFavorited { get; set; }

}