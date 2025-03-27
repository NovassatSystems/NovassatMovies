namespace NovassatMovies.Features.Movies;

public class ProductionCountryResponse
{
    [JsonPropertyName("iso_3166_1")]
    public string Iso31661 { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
