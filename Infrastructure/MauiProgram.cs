using CommunityToolkit.Maui;
using epj.RouteGenerator;
using FFImageLoading.Maui;
using Microsoft.Maui.Handlers;
using PanCardView;
using SkiaSharp.Views.Maui.Controls.Hosting;
//using Xe.AcrylicView;


#if ANDROID
using Color = Android.Graphics.Color;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
#endif

namespace NovassatMovies;
[AutoRoutes("Page")]
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseFFImageLoading()
            .UseCardsView()
            //.UseAcrylicView()
            .UseMauiCommunityToolkit()
            .ConfigurePages()
            .ConfigureViewModels()
            .ConfigureServices()
            .ConfigureOptions()
            .ConfigureCustomizations()
            .ConfigureCustomFonts();

        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDNROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#endif
        });

        return builder.Build();
	}
}
