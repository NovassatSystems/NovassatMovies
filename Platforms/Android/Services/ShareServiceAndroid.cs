using Android.Content;
namespace NovassatMovies.Infrastructure.Services;
public class ShareService : IShareService
{
    public Task ShareIMDbLink(string url)
    {
        var intent = new Intent(Intent.ActionSend);
        intent.SetType("text/plain");
        intent.PutExtra(Intent.ExtraText, url);

        var chooserIntent = Intent.CreateChooser(intent, "Compartilhar via");
        chooserIntent.SetFlags(ActivityFlags.NewTask);
        Android.App.Application.Context.StartActivity(chooserIntent);
        return Task.CompletedTask;
    }
}

