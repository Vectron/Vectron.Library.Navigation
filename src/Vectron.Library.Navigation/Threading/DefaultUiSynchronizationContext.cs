namespace Vectron.Library.Navigation.Threading;

/// <summary>
/// Default implementation of <see cref="IUiSynchronizationContext"/>.
/// </summary>
internal sealed class DefaultUiSynchronizationContext : IUiSynchronizationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultUiSynchronizationContext"/> class.
    /// </summary>
    public DefaultUiSynchronizationContext()
        => UiContext = SynchronizationContext.Current ?? new SynchronizationContext();

    /// <inheritdoc/>
    public SynchronizationContext UiContext
    {
        get;
    }

    /// <inheritdoc/>
    public void Post(Action action) => UiContext.Post(state => action.Invoke(), state: null);

    /// <inheritdoc/>
    public void Post<T>(Action<T> action, T state) => UiContext.Post(s => action.Invoke((T)s!), state);

    /// <inheritdoc/>
    public void Post<T, TResult>(Func<T, TResult> action, T state) => UiContext.Post(s => action.Invoke((T)s!), state);

    /// <inheritdoc/>
    public void Post<TResult>(Func<TResult> action) => UiContext.Post(state => action.Invoke(), state: null);
}
