using NovassatMovies.Infrastructure.Services;
using System.Globalization;

namespace NovassatMovies;

public partial class App : Application
{
    public App(ConnectivityService connectivityService)
    {
        InitializeComponent();
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        MainPage = new AppShell();
    }
}
