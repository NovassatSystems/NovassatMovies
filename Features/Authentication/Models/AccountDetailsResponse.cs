

namespace NovassatMovies.Features.Authentication;

public class AccountDetailsResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("include_adult")]
    public bool IncludeAdult { get; set; }

    [JsonPropertyName("iso_639_1")]
    public string Iso6391 { get; set; }

    [JsonPropertyName("iso_3166_1")]
    public string Iso31661 { get; set; }

    [JsonPropertyName("avatar")]
    public Avatar Avatar { get; set; }
}
