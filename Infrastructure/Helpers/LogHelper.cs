using System.Text;

namespace NovassatMovies.Infrastructure.Helpers;

public static class LogHelper
{
    public static void Log(string tag, Exception ex)
        => Log(tag, ConcatException(ex));

    static string ConcatException(Exception ex, StringBuilder stringBuilder = null)
    {
       if(stringBuilder is null)
            stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Message: {ex.Message}");
        stringBuilder.AppendLine($"StackTrace: {ex.StackTrace}");
        if (ex.InnerException != null)
            ConcatException(ex.InnerException, stringBuilder);
        return stringBuilder.ToString();
    }

    public static void Log(string tag, string message)
    {
#if DEBUG
        Console.WriteLine($"{tag}: {message}");
#endif
    }
}
