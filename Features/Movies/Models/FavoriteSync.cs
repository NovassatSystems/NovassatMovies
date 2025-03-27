using SQLite;

namespace NovassatMovies.Features.Movies;

public class FavoriteSync
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int MovieId { get; set; }
}
