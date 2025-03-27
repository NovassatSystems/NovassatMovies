using NovassatMovies.Infrastructure.Providers;

namespace NovassatMovies.Infrastructure.Extensions;

public static class JsonSerializerExtensions
{
    public static string Serialize<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptionsProvider.Options);
    }

    public static T Deserialize<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptionsProvider.Options);
    }
}