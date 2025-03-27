using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NovassatMovies.Infrastructure.Services;

namespace NovassatMovies.Features.MovieDetail;

public partial class MovieDetailPage : BasePage
{
	public MovieDetailPage(MovieDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
