using NovassatMovies.Infrastructure;
using System.Collections.ObjectModel;

namespace NovassatMovies.Features.Search;

public partial class SearchMovieViewModel : BaseViewModel, IQueryAttributable
{
    #region Services
    readonly IMoviesService _moviesService;
    readonly IAuthenticationService _authenticationService;
    readonly DatabaseService _databaseService;
    #endregion

    #region Properties
    [ObservableProperty] public ObservableCollection<Movie> searchResults = new();
    [ObservableProperty] public Movie movieDetail;
    #endregion

    #region Ctor
    public SearchMovieViewModel(IMoviesService moviesService, IAuthenticationService authenticationService, DatabaseService databaseService)
    {
        _moviesService = moviesService;
        _authenticationService = authenticationService;
        _databaseService = databaseService;
    }
    #endregion

    #region Methods
    public async Task SearchMoviesAsync(string query)
    {
        SearchResults.Clear();

        await foreach (var movie in _moviesService.SearchMoviesAsync(query))
        {
            SearchResults.Add(movie);
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task PerformSearch(string query)
    {
        await SearchMoviesAsync(query);
    }

    [RelayCommand]
    public async Task GetMovieDetailAsync(int movieId)
    {

        var active = await _authenticationService.IsSessionActiveAsync();

        if (!active)
        {
            await Toast.Make("Sua sessão expirou e será redirecionado para a tela de Login ").Show();
            return;
        }

        var detail = await _moviesService.GetMovieDetailAsync(movieId).Handle(this, true);

        var movie = await _databaseService.IsFavorite(movieId);

        MovieDetail = detail.Data;
        MovieDetail.IsFavorite = movie is not null ? movie.IsFavorite : false;
        var movieDetailJson = MovieDetail.Serialize();
        Preferences.Set("MovieDetailed", movieDetailJson);
        await Shell.Current.GoToAsync(Routes.MovieDetailPage);
    }

    #endregion
}

