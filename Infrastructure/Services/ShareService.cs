namespace NovassatMovies.Infrastructure.Services;

//public partial class ShareService
//{
//    public partial Task Share(string text);
//}

public interface IShareService
{
    Task ShareIMDbLink(string url);
}