namespace NovassatMovies.Features.Movies;

public partial class MovieDetailResponse :ObservableObject
{

    string _fullPosterPath;
    public string FullPosterPath
    {
        get => _fullPosterPath ?? $"https://image.tmdb.org/t/p/w500{PosterPath}";
        set => SetProperty(ref _fullPosterPath, value);
    }

    string _fullBackdropPath;
    public string FullBackdropPath
    {
        get => _fullBackdropPath ?? $"https://image.tmdb.org/t/p/w500{BackdropPath}";
        set => SetProperty(ref _fullBackdropPath, value);
    }

    string _formattedGenres;
    public string FormattedGenres
    {
        get => _formattedGenres ?? string.Join(" • ", Genres?.Select(g => g.Name) ?? Enumerable.Empty<string>());
        set => SetProperty(ref _formattedGenres, value);
    }

    string _formattedRuntime;
    public string FormattedRuntime
    {
        get => _formattedRuntime ?? $"{Runtime / 60}h {Runtime % 60}m";
        set => SetProperty(ref _formattedRuntime, value);
    }

    [ObservableProperty] public bool isFavorite;

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; }

    [JsonPropertyName("budget")]
    public int Budget { get; set; }

    [JsonPropertyName("genres")]
    public List<GenreResponse> Genres { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string ImdbId { get; set; }

    [JsonPropertyName("origin_country")]
    public List<string> OriginCountry { get; set; }

    [JsonPropertyName("original_language")]
    public string OriginalLanguage { get; set; }

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }

    [JsonPropertyName("overview")]
    public string Overview { get; set; }

    [JsonPropertyName("popularity")]
    public double Popularity { get; set; }

    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; }

    [JsonPropertyName("revenue")]
    public int Revenue { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("tagline")]
    public string Tagline { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }
}