using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NovassatMovies.Features.Movies;

public partial class Movie : ObservableObject
{
    [JsonPropertyName("id")]
    [PrimaryKey]
    public int Id { get; set; }

    [JsonPropertyName("adult")]
    public bool Adult { get; set; }

    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; }

    [JsonPropertyName("genre_ids")]
    [Ignore]
    public List<int> GenreIds { get; set; }

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
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("vote_average")]
    public double VoteAverage { get; set; }

    [JsonPropertyName("vote_count")]
    public int VoteCount { get; set; }

    public Category Category { get; set; }

    [ObservableProperty]
    public bool isFavorite;

    [JsonPropertyName("budget")]
    public int Budget { get; set; }

    [JsonPropertyName("genres")]
    [Ignore]
    public List<GenreResponse> Genres { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }

    [JsonPropertyName("imdb_id")]
    public string ImdbId { get; set; }

    [JsonPropertyName("revenue")]
    public int Revenue { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("tagline")]
    public string Tagline { get; set; }

    public string FullPosterPath => $"https://image.tmdb.org/t/p/w500{PosterPath}";
    public string FullBackdropPath => $"https://image.tmdb.org/t/p/w500{BackdropPath}";

    private string _formattedGenres;
    public string FormattedGenres
    {
        get => _formattedGenres ?? string.Join(" • ", Genres?.Select(g => g.Name) ?? Enumerable.Empty<string>());
        set => SetProperty(ref _formattedGenres, value);
    }

    private string _formattedRuntime;
    public string FormattedRuntime
    {
        get => _formattedRuntime ?? $"{Runtime / 60}h {Runtime % 60}m";
        set => SetProperty(ref _formattedRuntime, value);
    }

    public string GenresJson
    {
        get => Genres.Serialize();
        set => Genres = value.Deserialize<List<GenreResponse>>();
    }

    public byte[] PosterBytes { get; set; }
    public byte[] BackdropBytes { get; set; }

    [Ignore]
    public ImageSource Poster => PosterBytes != null ? ImageSource.FromStream(() => new MemoryStream(PosterBytes)) : null;

    [Ignore]
    public ImageSource Backdrop => BackdropBytes != null ? ImageSource.FromStream(() => new MemoryStream(BackdropBytes)) : null;

}
