namespace NovassatMovies.Features.Authentication;

public partial class AuthPage : BasePage
{
	public AuthPage(AuthViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

		Routing.RegisterRoute(Routes.MoviesPage, typeof(MoviesPage));
    }
}
