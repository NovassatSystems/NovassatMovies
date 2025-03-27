namespace NovassatMovies.Infrastructure.Services;

public class ConnectivityService
{
    public ConnectivityService() => Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

    void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
    {
        bool isConnected = e.NetworkAccess == NetworkAccess.Internet;
        WeakReferenceMessenger.Default.Send(new ConnectivityChangedMessage(isConnected));
    }
}
