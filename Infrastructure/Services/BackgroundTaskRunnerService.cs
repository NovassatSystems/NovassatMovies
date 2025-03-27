using System.Threading;

namespace NovassatMovies.Infrastructure.Services;

public enum BackgroundTaskStatus
{
    None, 
    Running, 
    Completed,
    Failed
}


#region Empty
public interface IBackgroundTaskEmptyRunnerService
{
    event EventHandler<BackgroundTaskEmptyEventArgs> StatusChanged;
    void RunInBackground(Func<Task> backgroundTask);
}
public class BackgroundTaskEmptyRunnerService : IBackgroundTaskEmptyRunnerService, IDisposable
{
    public event EventHandler<BackgroundTaskEmptyEventArgs> StatusChanged;

    CancellationTokenSource _cancellationTokenSource;
    Task _runningTask;
    bool _isDisposed;

    public void RunInBackground(Func<Task> backgroundTask)
    {
        Dispose();

        _cancellationTokenSource = new CancellationTokenSource(20000);
        _runningTask = Task.Run(async () => {
            try
            {
                OnStatusChanged(BackgroundTaskStatus.Running);
                await backgroundTask();
                OnStatusChanged(BackgroundTaskStatus.Completed);
            }
            catch (Exception ex)
            {

                OnStatusChanged(BackgroundTaskStatus.Failed, ex);
            }
        }, _cancellationTokenSource.Token);
    }

    void OnStatusChanged(BackgroundTaskStatus status, Exception ex = null)
    {
        if(!_cancellationTokenSource.IsCancellationRequested)
            StatusChanged?.Invoke(this, new BackgroundTaskEmptyEventArgs(status, ex));
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _runningTask?.Dispose();
    }
}
#endregion

#region Typed

public interface IBackgroundTaskRunnerService<T>
{
    event EventHandler<BackgroundTaskEventArgs<T>> StatusChanged;
    void RunInBackground(Func<Task<T>> backgroundTask);
}

public class BackgroundTaskRunnerService<T> : IBackgroundTaskRunnerService<T>, IDisposable
{
    public event EventHandler<BackgroundTaskEventArgs<T>> StatusChanged;
    CancellationTokenSource _cancellationTokenSource;
    Task _runningTask;
    bool _isDisposed;
    public void RunInBackground(Func<Task<T>> backgroundTask)
    {
        Dispose();

        _cancellationTokenSource = new CancellationTokenSource(20000);
        _runningTask = Task.Run(async () =>
        {
            try
            {
                OnStatusChanged(BackgroundTaskStatus.Running, default);
                var result = await backgroundTask();
                OnStatusChanged(BackgroundTaskStatus.Completed, result);
            }
            catch (Exception ex)
            {
                OnStatusChanged(BackgroundTaskStatus.Failed, default, ex);
            }
        }, _cancellationTokenSource.Token);
    }

    void OnStatusChanged(BackgroundTaskStatus status, T result, Exception ex = null)
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
            StatusChanged?.Invoke(this, new BackgroundTaskEventArgs<T>(status, result, ex));
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _runningTask?.Dispose();
    }
}
#endregion

public record BackgroundTaskEventArgs<T>(BackgroundTaskStatus TaskStatus, T Result, Exception Error = null);
public record BackgroundTaskEmptyEventArgs(BackgroundTaskStatus TaskStatus, Exception Error = null);