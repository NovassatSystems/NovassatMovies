using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies;

public partial class MoviesPage : BasePage
{
    private IDispatcherTimer _timer;

    public MoviesPage(MoviesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        Routing.RegisterRoute(Routes.MovieDetailPage, typeof(MovieDetailPage));
        Routing.RegisterRoute(Routes.MoviesFavoritePage, typeof(MoviesFavoritePage));

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(3);
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    void OnTimerTick(object sender, EventArgs e)
    {
        carouselView.FlowDirection = FlowDirection.RightToLeft;
        carouselView.IsNextItemPanInteractionEnabled = true;
        carouselView.IsAutoNavigatingAnimationEnabled = true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer.Stop();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _timer.Start();
    }
}