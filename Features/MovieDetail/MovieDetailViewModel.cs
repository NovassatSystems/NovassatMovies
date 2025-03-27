using NovassatMovies.Infrastructure.Extensions;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Features.MovieDetail;

public partial class MovieDetailViewModel : BaseViewModel, IQueryAttributable
{
    #region Services
    readonly IMoviesService _moviesService;
    readonly IAuthenticationService _authenticationService;
    readonly IShareService _shareService;
    readonly DatabaseService _databaseService;
    #endregion

    #region Properties
    [ObservableProperty] public Movie movie;
    [ObservableProperty] public AccountDetailsResponse accountDetails;
    [ObservableProperty] public bool isOnline;
    #endregion

    #region Ctor
    public MovieDetailViewModel(IMoviesService moviesService, IAuthenticationService authenticationService, IShareService shareService, DatabaseService databaseService)
    {
        _moviesService = moviesService;
        _authenticationService = authenticationService;
        _shareService = shareService;
        _databaseService = databaseService;
        GetDetailedMovie();
        GetAccountDetails();

        WeakReferenceMessenger.Default.Register<ConnectivityChangedMessage>(this, (r, message) =>
        {
            MainThread.BeginInvokeOnMainThread(() => IsOnline = message.Value);
        });

        IsOnline = Connectivity.NetworkAccess == NetworkAccess.Internet;
        _databaseService = databaseService;
    }
    #endregion

    #region Methods
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("MovieDetail", out var movieId) && movieId is string movieIdStr)
        {
            if (int.TryParse(movieIdStr, out int id))
                GetMovieDetails(id);
        }
    }

    public void GetDetailedMovie()
    {
        var movieId = Preferences.Get("MovieDetailed", 0);
        GetMovieDetails(movieId);
        
    }

    public async void GetMovieDetails(int movieId)
    {
      
        var detail = await _moviesService.GetMovieDetailAsync(movieId).Handle(this, true);

        var movie = await _databaseService.IsFavorite(movieId);

        Movie = detail.Data;
        Movie.IsFavorite = movie is not null ? movie.IsFavorite : false;
    }

    void GetAccountDetails()
    {
        var accountDetailsSerialized = Preferences.Get("AccountDetails", null);
        if (accountDetailsSerialized != null)
        {
            var accountDetails = accountDetailsSerialized.Deserialize<AccountDetailsResponse>();
            AccountDetails = accountDetails;
        }
    }
    #endregion

    #region Commands
    [RelayCommand]
    public async Task AddOrRemoveFavoriteMovie(string add)
    {
        var active = await _authenticationService.IsSessionActiveAsync();

        if (!active)
        {
            await Toast.Make("Sua sessão expirou e será redirecionado para a tela de Login ").Show();
            return;
        }
        var result = await _moviesService.AddOrRemoveFavoriteMovieAsync(Movie.Id, add is "1");

        Movie.IsFavorite = result.WasFavorited;
    }

    [RelayCommand]
    public async Task ShareMovieAsync() => await _shareService.ShareIMDbLink($"https://www.imdb.com/pt/title/{Movie.ImdbId}");

    #endregion
}