

#if ANDROID
using Color = Android.Graphics.Color;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Views;
#endif
using Microsoft.Maui.LifecycleEvents;
using System.Text.Encodings.Web;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Infrastructure.Extensions;

internal static class MauiProgramExtensions
{
    public static MauiAppBuilder ConfigurePages(this MauiAppBuilder app)
    {
        app.Services.AddTransient<AuthPage>();
        app.Services.AddTransient<MoviesPage>();
        app.Services.AddTransient<MovieDetailPage>();
        app.Services.AddTransient<MoviesFavoritePage>();
        app.Services.AddTransient<SearchMoviePage>();
        return app;
    }

    public static MauiAppBuilder ConfigureViewModels(this MauiAppBuilder app)
    {
        app.Services.AddTransient<AuthViewModel>();
        app.Services.AddTransient<MoviesViewModel>();
        app.Services.AddTransient<MovieDetailViewModel>();
        app.Services.AddTransient<MoviesFavoriteViewModel>();
        app.Services.AddTransient<SearchMovieViewModel>();
        return app;
    }

    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder app)
    {
        app.Services.AddSingleton(new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "movies.db3")));
        app.Services.AddSingleton<ConnectivityService>();
        app.Services.AddSingleton<IMoviesService, MoviesService>();
        app.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        app.Services.AddSingleton<ISyncService, SyncService>();
#if ANDROID
        app.Services.AddSingleton<IShareService, ShareService>();
#elif IOS
        app.Services.AddSingleton<IShareService, ShareService>();
#endif
        return app;
    }

    public static MauiAppBuilder ConfigureOptions(this MauiAppBuilder app)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        JsonSerializerOptionsProvider.Options = jsonSerializerOptions;
        return app;
    }

    public static MauiAppBuilder ConfigureCustomFonts(this MauiAppBuilder app)
    {
        app.ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        return app;
    }

    public static MauiAppBuilder ConfigureCustomizations(this MauiAppBuilder app)
    {
        app.ConfigureLifecycleEvents(lifecycle =>
        {
#if ANDROID
            lifecycle.AddAndroid(android => android.OnCreate((activity, bundle) =>
            {
                var window = activity.Window;
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);

                window.DecorView.SystemUiVisibility =
                    (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LayoutFullscreen |
                    (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LayoutStable;

                window.SetStatusBarColor(Android.Graphics.Color.Transparent);

                var background = AndroidX.Core.Content.ContextCompat.GetDrawable(activity, Resource.Drawable.status_bar_gradient);
                window.SetBackgroundDrawable(background);
            }));
#endif
        });

        return app;
    }

}

