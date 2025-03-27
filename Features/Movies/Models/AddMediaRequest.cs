using SQLite;

namespace NovassatMovies.Features.Movies;

public class AddMediaRequest
{
    public AddMediaRequest(string mediaType, int mediaId, bool favorite)
    {
        MediaType = mediaType;
        MediaId = mediaId;
        Favorite = favorite;
    }
    [JsonPropertyName("media_type")]
    public string MediaType { get; set; }

    [JsonPropertyName("media_id")]
    public int MediaId { get; set; }

    [JsonPropertyName("favorite")]
    public bool Favorite { get; set; }
}
