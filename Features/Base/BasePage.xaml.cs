using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Features.Base;

public partial class BasePage : ContentPage
{
    public BasePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing(); 
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            DeviceSafeInsetsService d = new DeviceSafeInsetsService();
            double topArea = d.GetSafeAreaTop();
            double bottomArea = d.GetSafeAreaBottom();
            var safeInsets = On<iOS>().SafeAreaInsets();
            safeInsets.Top = topArea;
            safeInsets.Bottom = bottomArea;

            Padding = safeInsets;

            if (App.Current.Resources.TryGetValue("MangoTango", out var valuesMt) &&
            App.Current.Resources.TryGetValue("VividViolet", out var valuesVv))
            {
                Root.Background = new LinearGradientBrush()
                {
                    StartPoint = new() { X = 1, Y = 0 },
                    EndPoint = new() { X = 0, Y = 0 },
                    GradientStops = new()
                    {
                        new() { Offset = 0, Color = valuesMt as Color },
                        new() { Offset = 1, Color = valuesVv as Color }
                    }
                };
            }
        });
    }
}