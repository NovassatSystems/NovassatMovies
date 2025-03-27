using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Features.Search;

public partial class SearchMoviePage : BasePage
{
	SearchMovieViewModel ViewModel => BindingContext as SearchMovieViewModel;
	public SearchMoviePage(SearchMovieViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        Routing.RegisterRoute(Routes.MovieDetailPage, typeof(MovieDetailPage));
    }


    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
		ViewModel?.SearchMoviesAsync(e.NewTextValue);
    }
}