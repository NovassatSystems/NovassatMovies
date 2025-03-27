using UIKit;

namespace NovassatMovies.Infrastructure.Services;

public partial class DeviceSafeInsetsService
{
    public partial double GetSafeAreaTop()
    {
        if(UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
            var topPadding = window.SafeAreaInsets.Top;
            return topPadding;
        }
        return 0;
    }
    public partial double GetSafeAreaBottom()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
            var bottomPadding = window.SafeAreaInsets.Bottom;
            return bottomPadding;
        }
        return 0;
    }
}
