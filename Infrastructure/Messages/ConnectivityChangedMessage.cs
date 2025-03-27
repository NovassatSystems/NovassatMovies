using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NovassatMovies.Infrastructure.Messages;

public class ConnectivityChangedMessage : ValueChangedMessage<bool>
{
    public ConnectivityChangedMessage(bool isConnected) : base(isConnected) { }
}