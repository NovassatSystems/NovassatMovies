namespace NovassatMovies.Features.Favorite;

public partial class MoviesFavoriteViewModel : BaseViewModel, IQueryAttributable
{
    #region Services
    readonly IMoviesService _moviesService;
    readonly IAuthenticationService _authenticationService;
    #endregion

    #region BackgroundTasks

    #endregion

    #region Properties
    [ObservableProperty] public MoviesResponse favoriteMovies;
    [ObservableProperty] public Movie movieDetail;
    #endregion

    #region Ctor
    public MoviesFavoriteViewModel(IMoviesService moviesService, IAuthenticationService authenticationService)
    {
        _moviesService = moviesService;
        _authenticationService = authenticationService;
        GetFavoriteMovies();
    }
    #endregion

    #region Methods
    public void GetFavoriteMovies()
    {
        var favoriteMoviesSerialized = Preferences.Get("FavoriteMovies", null);
        if (favoriteMoviesSerialized != null)
        {
            var favoriteMovies = favoriteMoviesSerialized.Deserialize<MoviesResponse>();
            FavoriteMovies = favoriteMovies;
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task GetMovieDetailAsync(int movieId)
    {
        try
        { 
            Preferences.Set("MovieDetailed", movieId);
            await Shell.Current.GoToAsync(Routes.MovieDetailPage);
        }
        catch (Exception ex)
        {
            LogHelper.Log(nameof(MoviesFavoriteViewModel), ex);
        }
    }
    #endregion
}