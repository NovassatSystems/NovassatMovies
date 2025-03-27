namespace NovassatMovies.Infrastructure.Services;

public interface ISyncService
{
    Task<bool> SyncFavAsync(List<int> moviesToAddSync, List<int> moviesToRemoveSync);
    Task<(List<int>, List<int>)> FindToSyncFavAsync();
}

public class SyncService : ISyncService
{
    readonly DatabaseService _databaseService;
    readonly IMoviesService _moviesService;
    public SyncService(DatabaseService databaseService, IMoviesService moviesService)
    {
        _databaseService = databaseService;
        _moviesService = moviesService;
    }

    public async Task<(List<int>?, List<int>?)> FindToSyncFavAsync()
    {
        try
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                return (default, default);

            var localFavorites = await _databaseService.GetFavoriteMoviesAsync();
            var localFavoriteIds = localFavorites.Select(f => f.Id).ToList();

            var onlineFavorites = await _moviesService.GetFavoriteMoviesAsync();
            var onlineFavoriteIds = onlineFavorites.Results.Select(f => f.Id).ToList();

            //var lastSyncedFavorites = await _databaseService.GetSyncedFavoritesAsync();

            var moviesToAddSync = localFavoriteIds.Except(onlineFavoriteIds).ToList();
            //var moviesToRemoveSync = lastSyncedFavorites.Except(localFavoriteIds).ToList();

            return (moviesToAddSync, new List<int>());
        }
        catch (Exception ex)
        {
            return (default, default);
        }
    }

    public async Task<bool> SyncFavAsync(List<int> moviesToAddSync, List<int> moviesToRemoveSync)
    {
        //Sincronizar Adição
        foreach (var movieId in moviesToAddSync)
        {
            await _moviesService.AddOrRemoveFavoriteMovieAsync(movieId, true);
        }

        //Sincronizar Remoção
        foreach (var movieId in moviesToRemoveSync)
        {
            await _moviesService.AddOrRemoveFavoriteMovieAsync(movieId, false);
        }
        //Sincronizar Favoritos

        await _moviesService.GetFavoriteMoviesAsync();

        return true;
    }

}
