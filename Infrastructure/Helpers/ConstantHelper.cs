namespace NovassatMovies.Infrastructure.Helpers;

public static class ConstantHelper
{
    public const string BaseUrl = "https://api.themoviedb.org/3";    
    public const string AuthUrl = "https://www.themoviedb.org/authenticate";
    public const string ApiKey = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0OTQwMTUyYWQyYjhiYWYxMjgzNGVkNGQ5YjI1MWI5ZCIsIm5iZiI6MTc0MjgyNDkzNi43MzYsInN1YiI6IjY3ZTE2NWU4MWYyNTZhZWYxY2M3MTBhNSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.Qh0vbFt6ZHLwCH1Q1sbBwwa2z1DA1E9ShHldFzbcPIU";
    public const string RedirectUri = "novassatmovies://auth-callback";

    public const string NoInternetConnection = "Algo deu errado com a conexão com a internet. Por favor, tente novamente mais tarde.";
    public const string GenericError = "Algo deu errado. Por favor, tente novamente mais tarde.";
}
