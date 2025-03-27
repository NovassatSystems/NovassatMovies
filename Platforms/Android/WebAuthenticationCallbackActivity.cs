using Android.App;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;
using NovassatMovies.Infrastructure.Messages;
using System.Web;

namespace NovassatMovies.Platforms.Droid
{
    [Activity(Exported = true, LaunchMode = LaunchMode.SingleTop, Name = "com.novassatsystems.novassatmovies.WebAuthenticationCallbackActivity")]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
                  DataScheme = "novassatmovies",
                  DataHost = "auth-callback")]
    public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var data = Intent?.Data?.ToString();
            if (!string.IsNullOrEmpty(data) && data.StartsWith("novassatmovies://auth-callback"))
            {
                HandleCallback(data);
            }

            Finish();
        }

        void HandleCallback(string url)
        {
            var requestToken = ExtractToken(url);
            WeakReferenceMessenger.Default.Send(new OAuthCallbackMessage(requestToken));
        }

        string ExtractToken(string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query["request_token"];
        }
    }
}
