using Foundation;
using UIKit;

namespace NovassatMovies.Infrastructure.Services;

public class ShareService : IShareService
{
    public Task ShareIMDbLink(string url)
    {
        var items = new NSObject[] { new NSString(url) };
        var activityController = new UIActivityViewController(items, null);

        var rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;
        rootController.PresentViewController(activityController, true, null);

        return Task.CompletedTask;
        
    }
}