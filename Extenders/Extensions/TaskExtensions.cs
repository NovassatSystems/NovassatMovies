namespace NovassatMovies.Extenders.Extensions;

public static class TaskExtensions
{
    public static async Task<(bool Success, T Data)> Handle<T>(this Task<T> self, BaseViewModel vm = null, bool forceGet = false, string loadingMessage = "")
    {
        if (forceGet)
        {
            var result = await self.ConfigureAwait(false);
            return (true, result);
        }
       
        if(Connectivity.NetworkAccess != NetworkAccess.Internet)
        {
            ShowToast(ConstantHelper.NoInternetConnection);
            return (false, default(T));
        }

        vm?.SetLoading(true);
        try
        {
            var result = await self.ConfigureAwait(false);
            return (true, result);
        }
        catch(FlurlHttpException flurlEx)
        {
            HandleException(flurlEx);
        }
        catch (Exception ex)
        {

            HandleException(ex);
        }
        finally
        {
            vm?.SetLoading(false);
        }

        return (false, default(T));
    }

    static void HandleException(Exception ex)
    {
        LogHelper.Log(nameof(TaskExtensions), ex);
        ShowToast(ConstantHelper.GenericError);
    }

    static void ShowToast(string message)
    {
        MainThread.BeginInvokeOnMainThread(() => Toast.Make(message).Show());
    }
}
