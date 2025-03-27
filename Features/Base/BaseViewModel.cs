namespace NovassatMovies.Features.Base;

public partial class BaseViewModel : ObservableObject, IQueryAttributable
{
	#region Properties
	[ObservableProperty] public bool isLoading;
	[ObservableProperty] public string loadingMessage;
	#endregion

	#region Methods
	public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
	{

	}

	public void SetLoading(bool value, string message = "")
	{
		IsLoading = value;
		LoadingMessage = message;
	}
	#endregion

	#region Commands
	[RelayCommand]
	public async Task GoBackAsync() => await Shell.Current.GoToAsync("..");
	#endregion
}
