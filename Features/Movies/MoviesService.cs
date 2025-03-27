


namespace NovassatMovies.Features.Movies;

public interface IMoviesService
{
    Task<List<Movie>> GetPopularMoviesAsync();
    Task<List<Movie>> GetTopRatedMoviesAsync();
    Task<List<Movie>> GetTrendingMoviesAsync();
    Task<Movie> GetMovieDetailAsync(int movieId);
    Task<MoviesResponse> GetFavoriteMoviesAsync();
    Task<AddOrRemoveResponse> AddOrRemoveFavoriteMovieAsync(int movieId, bool favorite);
    IAsyncEnumerable<Movie> SearchMoviesAsync(string query);
    Task SaveDetailsForMovies(List<Movie> moviesToSave);
    IAsyncEnumerable<int> DownloadAndSaveImagesAsync(List<Movie> movies);
}
public class MoviesService : IMoviesService
{
    readonly DatabaseService _databaseService;

    public MoviesService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<Movie>> GetPopularMoviesAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return await _databaseService.GetMoviesByCategoryAsync(Category.Popular);

        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/movie/popular")
            .SetQueryParam("language", "pt-BR")
            .SetQueryParam("page", 1)
            .SetQueryParam("sort_by", "vote_average.desc")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<MoviesResponse>();

        response.Results.ForEach(x => x.Category = Category.Popular);
        await _databaseService.SaveMoviesAsync(response.Results);

        return response.Results;
    }
    
    public async Task<List<Movie>> GetTopRatedMoviesAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return await _databaseService.GetMoviesByCategoryAsync(Category.TopRated);

        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/movie/top_rated")
            .SetQueryParam("language", "pt-BR")
            .SetQueryParam("page", 1)
            .SetQueryParam("sort_by", "vote_average.desc")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<MoviesResponse>();
        response.Results.ForEach(x => x.Category = Category.TopRated);
        await _databaseService.SaveMoviesAsync(response.Results);
        return response.Results;
    }

    public async Task<List<Movie>> GetTrendingMoviesAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            return await _databaseService.GetMoviesByCategoryAsync(Category.Trending);

        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/trending/movie/day")
            .SetQueryParam("language", "pt-BR")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<MoviesResponse>();

        response.Results.ForEach(x => x.Category = Category.Trending);
        await _databaseService.SaveMoviesAsync(response.Results);
        return response.Results;
    }

    public async Task<Movie> GetMovieDetailAsync(int movieId)
    {
        if(Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            var movie = await _databaseService.GetMovieAsync(movieId);
            return movie;
        }

        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment($"/movie/{movieId}")
            .SetQueryParam("language", "pt-BR")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<Movie>();
        return response;
    }

    public async Task<MoviesResponse> GetFavoriteMoviesAsync()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            var result = await _databaseService.GetFavoriteMoviesAsync();
            return new MoviesResponse { Results = result };
        }

        var accountDetailsSerialized = Preferences.Get("AccountDetails", null);
        if (accountDetailsSerialized == null)
            return null;
        var accountDetail = accountDetailsSerialized.Deserialize<AccountDetailsResponse>();
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment($"/account/{accountDetail.Id}/favorite/movies")
            .SetQueryParam("language", "pt-BR")
            .SetQueryParam("page", 1)
            .SetQueryParam("sort_by", "created_at.desc")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<MoviesResponse>();
        response.Results.ForEach(x => x.IsFavorite = true);
        await _databaseService.SaveMoviesAsync(response.Results);
        //await _databaseService.SaveSyncedFavoritesAsync(response?.Results?.Select(x => x.Id).ToList());
        return response;
    }

    public async Task<AddOrRemoveResponse> AddOrRemoveFavoriteMovieAsync(int movieId, bool favorite)
    {
        var response = new AddOrRemoveResponse();

        
        bool isOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

       
        var movie = await _databaseService.GetMovieAsync(movieId);
        if (movie == null) return response;

        
        if (!isOnline)
        {
            movie.IsFavorite = favorite;
            await _databaseService.SaveMovieAsync(movie);

            var favoriteMoviesSerialized = Preferences.Get("FavoriteMovies", null);
            if (favoriteMoviesSerialized != null)
            {
                var favoriteMovies = favoriteMoviesSerialized.Deserialize<MoviesResponse>();
                favoriteMovies.Results.Add(movie);
                Preferences.Set("FavoriteMovies", favoriteMovies.Serialize());
            }

            return new AddOrRemoveResponse { StatusCode = favorite ? 1 : 13, Success = true,  WasFavorited = favorite};
        }

        
        var accountDetailsSerialized = Preferences.Get("AccountDetails", null);
        if (accountDetailsSerialized == null) return response;

        var accountDetails = accountDetailsSerialized.Deserialize<AccountDetailsResponse>();


        var requestBody = new AddMediaRequest("movie", movieId, favorite);

        try
        {
            response = await ConstantHelper.BaseUrl
                .AppendPathSegment($"/account/{accountDetails.Id}/favorite")
                .WithHeader("accept", "application/json")
                .WithOAuthBearerToken(ConstantHelper.ApiKey)
                .PostJsonAsync(requestBody)
                .ReceiveJson<AddOrRemoveResponse>();

            var result = await GetFavoriteMoviesAsync();
            Preferences.Set("FavoriteMovies", result.Serialize());

            movie.IsFavorite = favorite;
            await _databaseService.SaveMovieAsync(movie);

            response.WasFavorited = response.StatusCode == 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao favoritar/desfavoritar filme: {ex.Message}");
        }

        return response;
    }


    public async IAsyncEnumerable<Movie> SearchMoviesAsync(string query)
    {
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/search/movie")
            .SetQueryParam("query", query)
            .SetQueryParam("language", "pt-BR")
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<MoviesResponse>();

        foreach (var movie in response.Results)
        {
            yield return movie;
        }
    }

    public async Task SaveDetailsForMovies(List<Movie> moviesToSave)
    {
        foreach (var movie in moviesToSave)
        {
            var detail = await GetMovieDetailAsync(movie.Id);
            movie.IsFavorite = await _databaseService.IsFavorite(movie.Id) is not null;
            movie.Overview = detail.Overview;
            movie.ReleaseDate = detail.ReleaseDate;
            movie.VoteAverage = detail.VoteAverage;
            movie.Genres = detail.Genres;
            movie.Runtime = detail.Runtime;
            movie.PosterPath = detail.PosterPath;
            await _databaseService.SaveMovieAsync(movie);
        }
    }

    public async IAsyncEnumerable<int> DownloadAndSaveImagesAsync(List<Movie> movies)
    {
        int remainingMovies = movies.Count;

        foreach (var movie in movies)
        {
            if (!string.IsNullOrEmpty(movie.FullBackdropPath))
            {
                movie.BackdropBytes = await movie.FullBackdropPath.GetBytesAsync();
            }

            if (!string.IsNullOrEmpty(movie.FullPosterPath))
            {
                movie.PosterBytes = await movie.FullPosterPath.GetBytesAsync();
            }

            await _databaseService.SaveMovieAsync(movie);

            remainingMovies--;

            // Retorna o valor do restante a cada iteração
            yield return remainingMovies;
        }
    }
}