namespace NovassatMovies.Features.Authentication;

public interface IAuthenticationService
{
    Task<string> GetRequestTokenAsync();
    Task<CreateSessionResponse> CreateSessionAsync(string requestToken);
    Task<AccountDetailsResponse> GetAccountDetailsAsync(string sessionId);
}
public class AuthenticationService : IAuthenticationService
{
    #region Token
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
    #endregion

    #region Session
    public async Task<CreateSessionResponse> CreateSessionAsync(string requestToken)
    {
        var response = await ConstantHelper.BaseUrl
            .AppendPathSegment("/authentication/session/new")
            .WithOAuthBearerToken(ConstantHelper.ApiKey)
            .PostJsonAsync(new CreateSessionRequest { RequesToken = requestToken})
            .ReceiveJson<CreateSessionResponse>();
        return response;
    }
    #endregion

    #region Account
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
    #endregion
}