namespace NovassatMovies.Features.Authentication;

public interface IAuthenticationService
{
    Task<string> GetRequestTokenAsync();
    Task<CreateSessionResponse> CreateSessionAsync(string requestToken);
    Task<AccountDetailsResponse> GetAccountDetailsAsync(string sessionId);
    Task<bool> IsSessionActiveAsync();
}
public class AuthenticationService : IAuthenticationService
{
    public async Task<bool> IsSessionActiveAsync()
    {
        //var sessionId = Preferences.Get("SessionId", null);
        //if (string.IsNullOrEmpty(sessionId))
        //    return false;


        //    var response = await ConstantHelper.BaseUrl
        //        .AppendPathSegment("/authentication/session")
        //        .SetQueryParam("session_id", sessionId)
        //        .WithHeader("accept", "application/json")
        //        .WithOAuthBearerToken(ConstantHelper.ApiKey)
        //        .GetJsonAsync<dynamic>();

        //    return response.success;
        return true;
       
    }
    public async Task<string> GetRequestTokenAsync()
    {
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/authentication/token/new")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .AllowAnyHttpStatus()
            .GetAsync()
            .ReceiveJson<RequestTokenResponse>();
        var url = $"{ConstantHelper.AuthUrl}/{response.RequestToken}?redirect_to={ConstantHelper.RedirectUri}";
        return url;
    }

    public async Task<CreateSessionResponse> CreateSessionAsync(string requestToken)
    {
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/authentication/session/new")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .PostJsonAsync(new CreateSessionRequest { RequesToken = requestToken})
            .ReceiveJson<CreateSessionResponse>();
        return response;
    }

    public async Task<AccountDetailsResponse> GetAccountDetailsAsync(string sessionId)
    {
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/account")
            .SetQueryParam("session_id", sessionId)
            .WithHeader("accept", "application/json")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .GetJsonAsync<AccountDetailsResponse>();
        return response;
    }
}