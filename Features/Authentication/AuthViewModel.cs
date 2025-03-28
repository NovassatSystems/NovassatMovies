namespace NovassatMovies.Features.Authentication;

public partial class AuthViewModel : BaseViewModel
{
    #region Services
    readonly IAuthenticationService _authenticationService;
    #endregion

    #region Properties
    [ObservableProperty] public AccountDetailsResponse accountDetails;
    #endregion

    #region Ctor
    public AuthViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
        RegisterMessages();
    }
    #endregion

    #region Messages
    void RegisterMessages()
    {
        try
        {

       
        WeakReferenceMessenger.Default.Register<OAuthCallbackMessage>(this, async (r, message) =>
        {
            var token = message.Value;
            Preferences.Set("AccessToken", token);
            var session = await _authenticationService.CreateSessionAsync(token).Handle(this);
            if (session.Success && session.Data.Success)
            {
                Preferences.Set("SessionId", session.Data.SessionId);
                var accountDetails = await _authenticationService.GetAccountDetailsAsync(session.Data.SessionId).Handle(this);
                if (accountDetails.Success)
                {
                    Preferences.Set("AccountDetails", accountDetails.Data.Serialize());
                    AccountDetails = accountDetails.Data;

                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        Shell.Current.FindByName<TabBar>("MainTabBar").IsVisible = true;
                        await Shell.Current.GoToAsync("//MainTabBar");
                        return;
                    }

                    Shell.Current.FindByName<FlyoutItem>("MainFlyout").IsVisible = true;
                    await Shell.Current.GoToAsync("//MainFlyout");
                }
            }
        });
        }
        catch (Exception ex)
        {
            LogHelper.Log(nameof(AuthViewModel), ex);
        }
    }
    #endregion

    #region Commands

    [RelayCommand]
    public async Task AuthAsync()
    {
        try
        {

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            var result = await Shell.Current.DisplayAlert("Sem Conexão", "Você está sem acesso à internet. Deseja tentar novamente ou acessar o modo offline?", "Tentar Novamente", "Modo Offline");
            if (result)
            {
                await AuthAsync();
                return;
            }
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                Shell.Current.FindByName<TabBar>("MainTabBar").IsVisible = true;
                await Shell.Current.GoToAsync("//MainTabBar");
                return;
            }

            Shell.Current.FindByName<FlyoutItem>("MainFlyout").IsVisible = true;
            await Shell.Current.GoToAsync("//MainFlyout");

            return;
        }
        var url = await _authenticationService.GetRequestTokenAsync().Handle(this);
        await Launcher.OpenAsync(url.Data);
        }
        catch (Exception ex)
        {
            LogHelper.Log(nameof(AuthViewModel), ex);
        }
    }
    #endregion
}