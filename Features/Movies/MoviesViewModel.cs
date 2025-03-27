using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NovassatMovies.Extenders.Extensions;
using NovassatMovies.Features.Base;
using NovassatMovies.Infrastructure;
using NovassatMovies.Infrastructure.Extensions;
using NovassatMovies.Infrastructure.Messages;
using NovassatMovies.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace NovassatMovies;

public partial class MoviesViewModel : BaseViewModel
{
    #region Services
    readonly IAuthenticationService _authenticationService;
    readonly IMoviesService _moviesService;
    readonly DatabaseService _databaseService;
    readonly ISyncService _syncService;
    #endregion

    #region BackgroundTasks
    BackgroundTaskRunnerService<MoviesResponse> _favoriteMoviesBackgroundTask;
    BackgroundTaskRunnerService<List<Movie>> _popularMoviesBackgroundTask;
    BackgroundTaskRunnerService<List<Movie>> _topRatedMoviesBackgroundTask;
    BackgroundTaskRunnerService<List<Movie>> _trendingMoviesBackgroundTask;

    BackgroundTaskRunnerService<List<int>> _getAllMoviesImages;
    BackgroundTaskEmptyRunnerService _getAllMovieDetails;
    #endregion

    #region Properties
    [ObservableProperty] public string authUrl;
    [ObservableProperty] public string resultado;
    [ObservableProperty] public string sessionId;
    [ObservableProperty] public string isConnected;
    [ObservableProperty] public string progressCount;

    [ObservableProperty] public bool showProgress;
    [ObservableProperty] public bool allMoviesDownloaded;
    [ObservableProperty] public bool gettingMoviesDetails;

    [ObservableProperty] public Movie movieDetail;

    [ObservableProperty] public ObservableCollection<Movie> popularMovies = new();
    [ObservableProperty] public ObservableCollection<Movie> topRatedMovies = new();
    [ObservableProperty] public ObservableCollection<Movie> trendingMovies = new();

    [ObservableProperty] public AccountDetailsResponse accountDetails;
    #endregion

    public MoviesViewModel(IAuthenticationService authenticationService,
        IMoviesService moviesService,
        DatabaseService databaseService,
        ISyncService syncService)

    {
        _authenticationService = authenticationService;
        _moviesService = moviesService;
        _databaseService = databaseService;
        _syncService = syncService;

        _favoriteMoviesBackgroundTask = new();
        _popularMoviesBackgroundTask = new();
        _topRatedMoviesBackgroundTask = new();
        _trendingMoviesBackgroundTask = new();
        _getAllMovieDetails = new();
        _getAllMoviesImages = new();

        GetAccountDetails();
        GetFavoriteMovies();

        GetTopRatedMoviesAsync();
        GetTrendingMoviesAsync();
        GetPopularMoviesAsync();

        WeakReferenceMessenger.Default.Register<ConnectivityChangedMessage>(this, (r, message) =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                GetTopRatedMoviesAsync();
                GetTrendingMoviesAsync();
                GetPopularMoviesAsync();
                IsConnected = message.Value ? "wifion" : "wifioff";
                if (message.Value)
                    await VerifySyncAsync();
            });
        });

        IsConnected = Connectivity.NetworkAccess == NetworkAccess.Internet ? "wifion" : "wifioff";
    }

    #region Methods
    async Task VerifySyncAsync()
    {
        var moviesToSync = await _syncService.FindToSyncFavAsync();
        var addCount = moviesToSync.Item1.Count;
        var removeCount = moviesToSync.Item2.Count;

        if (addCount > 0 || removeCount > 0)
        {
            string message = "Você";

            if (addCount > 0)
                message += $" favoritou {addCount} filme{(addCount > 1 ? "s" : "")}";

            if (addCount > 0 && removeCount > 0)
                message += " e";

            if (removeCount > 0)
                message += $" desfavoritou {removeCount} filme{(removeCount > 1 ? "s" : "")}";

            message += " enquanto estava offline. Faremos a sincronização neste instante";

            await Shell.Current.DisplayAlert("Sincronização", message, "Ok");

            await _syncService.SyncFavAsync(moviesToSync.Item1, moviesToSync.Item2).Handle(loadingMessage: "Aguarde estamos sincronizando seus favoritos");
        }
    }

    void GetAccountDetails()
    {
        var accountDetailsSerialized = Preferences.Get("AccountDetails", null);
        if (accountDetailsSerialized != null)
        {
            var accountDetails = JsonSerializer.Deserialize<AccountDetailsResponse>(accountDetailsSerialized);
            AccountDetails = accountDetails;
        }
    }

    #region Favorite
    public void GetFavoriteMovies()
    {
        _favoriteMoviesBackgroundTask.RunInBackground(_moviesService.GetFavoriteMoviesAsync);
        _favoriteMoviesBackgroundTask.StatusChanged += FavoriteMoviesBackgroundTask_StatusChanged;
    }

    private void FavoriteMoviesBackgroundTask_StatusChanged(object? sender, BackgroundTaskEventArgs<MoviesResponse> e)
    {
        if (e.TaskStatus is BackgroundTaskStatus.Completed && e.Result is not null)
        {
            var favoriteMoviesSerialized = e.Result.Serialize();
            Preferences.Set("FavoriteMovies", favoriteMoviesSerialized);
        }
    }
    #endregion

    #region Popular
    public void GetPopularMoviesAsync()
    {
        _popularMoviesBackgroundTask.RunInBackground(_moviesService.GetPopularMoviesAsync);
        _popularMoviesBackgroundTask.StatusChanged += PopularMoviesBackgroundTask_StatusChanged;

    }

    void PopularMoviesBackgroundTask_StatusChanged(object? sender, BackgroundTaskEventArgs<List<Movie>> e)
    {
        if (e.TaskStatus is BackgroundTaskStatus.Completed && e.Result is not null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                PopularMovies.Clear();
                e.Result.Where(movie => !string.IsNullOrWhiteSpace(movie.PosterPath)).ToList().ForEach(PopularMovies.Add);
                AllMoviesDownloaded = PopularMovies.Count > 0 && TopRatedMovies.Count > 0 && TrendingMovies.Count > 0;
                if (AllMoviesDownloaded)
                    GetAllMoviesDetails();
            });
        }
    }
    #endregion

    #region TopRated
    public void GetTopRatedMoviesAsync()
    {
        _topRatedMoviesBackgroundTask.RunInBackground(_moviesService.GetTopRatedMoviesAsync);
        _topRatedMoviesBackgroundTask.StatusChanged += TopRatedMoviesBackgroundTask_StatusChanged;

    }

    void TopRatedMoviesBackgroundTask_StatusChanged(object? sender, BackgroundTaskEventArgs<List<Movie>> e)
    {
        if (e.TaskStatus is BackgroundTaskStatus.Completed && e.Result is not null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                TopRatedMovies.Clear();
                e.Result.Where(movie => !string.IsNullOrWhiteSpace(movie.PosterPath)).ToList().ForEach(TopRatedMovies.Add);
                AllMoviesDownloaded = PopularMovies.Count > 0 && TopRatedMovies.Count > 0 && TrendingMovies.Count > 0;
                if (AllMoviesDownloaded)
                    GetAllMoviesDetails();
            });
        }
    }
    #endregion

    #region Trending
    public void GetTrendingMoviesAsync()
    {
        _trendingMoviesBackgroundTask.RunInBackground(_moviesService.GetTrendingMoviesAsync);
        _trendingMoviesBackgroundTask.StatusChanged += TrendingMoviesBackgroundTask_StatusChanged;
    }

    void TrendingMoviesBackgroundTask_StatusChanged(object? sender, BackgroundTaskEventArgs<List<Movie>> e)
    {
        if (e.TaskStatus is BackgroundTaskStatus.Completed && e.Result is not null)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                TrendingMovies.Clear();
                e.Result.Where(movie => !string.IsNullOrWhiteSpace(movie.PosterPath)).ToList().ForEach(TrendingMovies.Add);
                AllMoviesDownloaded = PopularMovies.Count > 0 && TopRatedMovies.Count > 0 && TrendingMovies.Count > 0;
                if (AllMoviesDownloaded)
                    GetAllMoviesDetails();
            });
        }
    }
    #endregion

    #region AllMoviesDetail(Background)
    void GetAllMoviesDetails()
    {
        if (GettingMoviesDetails)
            return;

        GettingMoviesDetails = true;

        _getAllMovieDetails.RunInBackground(async () =>
        {
            var movies = new List<Movie>();
            movies.AddRange(PopularMovies);
            movies.AddRange(TopRatedMovies);
            movies.AddRange(TrendingMovies);
            await _moviesService.SaveDetailsForMovies(movies);
        });
        _getAllMovieDetails.StatusChanged += GetAllMovieDetails_StatusChanged;
    }

    private void GetAllMovieDetails_StatusChanged(object? sender, BackgroundTaskEmptyEventArgs e)
    {
        if (e.TaskStatus is not BackgroundTaskStatus.Running)
        {
            MainThread.BeginInvokeOnMainThread(() => GettingMoviesDetails = false);
            
        }
    }




    #endregion

    #endregion

    #region Commands

    [RelayCommand]
    public async Task DownloadImagesAsync()
    {
        var movies = new List<Movie>();
        movies.AddRange(PopularMovies);
        movies.AddRange(TopRatedMovies);
        movies.AddRange(TrendingMovies);

        ShowProgress = true;

        await foreach (var remaining in _moviesService.DownloadAndSaveImagesAsync(movies))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ProgressCount = $"{remaining} ";
            });
        }

        await Shell.Current.DisplayAlert("Sincronização", "Imagens baixadas com sucesso", "Ok");
        ShowProgress = false;
    }

    
    

    [RelayCommand]
    public async Task Authenticate()
    {
        var requestToken = await _authenticationService.GetRequestTokenAsync();
        AuthUrl = $"{ConstantHelper.AuthUrl}/{requestToken}?redirect_to={ConstantHelper.RedirectUri}";

        await Launcher.OpenAsync(AuthUrl);
    }


    [RelayCommand]
    public async Task GetMovieDetailAsync(int movieId)
    {
        Preferences.Set("MovieDetailed", movieId);
        await Shell.Current.GoToAsync(Routes.MovieDetailPage);
    }
    #endregion
}