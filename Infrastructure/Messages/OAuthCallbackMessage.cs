using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NovassatMovies.Infrastructure.Messages;

public class OAuthCallbackMessage : ValueChangedMessage<string>
{
    public OAuthCallbackMessage(string url) : base(url) { }
}
