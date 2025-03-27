using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Features.Favorite;

public partial class MoviesFavoritePage : BasePage
{
	MoviesFavoriteViewModel ViewModel => BindingContext as MoviesFavoriteViewModel;
	public MoviesFavoritePage(MoviesFavoriteViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		ViewModel.GetFavoriteMovies();
    }
}
