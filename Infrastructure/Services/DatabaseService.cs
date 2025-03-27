using SQLite;
using System.IO;
using System.Threading.Tasks;

namespace NovassatMovies.Infrastructure
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Movie>().Wait();
            _database.CreateTableAsync<FavoriteSync>().Wait();
        }

        public Task<Movie> GetMovieAsync(int movieId) => _database.Table<Movie>().FirstOrDefaultAsync(m => m.Id == movieId);
        public Task<List<Movie>> GetMoviesAsync() => _database.Table<Movie>().ToListAsync();
        public Task<List<Movie>> GetMoviesByCategoryAsync(Category category) => _database.Table<Movie>().Where(m => m.Category == category)?.ToListAsync();
        public Task<List<Movie>> GetFavoriteMoviesAsync() => _database.Table<Movie>().Where(m => m.IsFavorite)?.ToListAsync();
        public Task<int> SaveMovieAsync(Movie movie) => _database.InsertOrReplaceAsync(movie);
        public Task<int> DeleteMovieAsync(Movie movie) => _database.DeleteAsync(movie);
        public async Task SaveMoviesAsync(List<Movie> movies) => movies.ForEach(movie => _database.InsertOrReplaceAsync(movie));
        public Task<Movie> IsFavorite(int movieId) => _database.Table<Movie>().Where(m => m.Id == movieId && m.IsFavorite).FirstOrDefaultAsync();
        public async Task SaveSyncedFavoritesAsync(List<int> favoriteMovieIds)
        {
           
            await _database.DeleteAllAsync<FavoriteSync>();
            foreach (var movieId in favoriteMovieIds)
            {
                await _database.InsertOrReplaceAsync(new FavoriteSync { MovieId = movieId });
            }
        }

        public async Task<List<int>> GetSyncedFavoritesAsync()
        {
            var records = await _database.Table<FavoriteSync>().ToListAsync();
            return records?.Select(r => r.MovieId).ToList();
        }
    }
}