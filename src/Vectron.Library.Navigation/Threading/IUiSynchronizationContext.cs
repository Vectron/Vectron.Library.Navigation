namespace Vectron.Library.Navigation.Threading;

/// <summary>
/// An abstraction over <see cref="SynchronizationContext"/>.
/// </summary>
public interface IUiSynchronizationContext
{
    /// <summary>
    /// Gets the <see cref="SynchronizationContext"/> of the ui thread.
    /// </summary>
    SynchronizationContext UiContext
    {
        get;
    }

    /// <summary>
    /// Dispatch an asynchronous message to a synchronization context.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    void Post(Action action);

    /// <inheritdoc cref="Post(Action)"/>
    /// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
    /// <param name="action"><inheritdoc cref="Post(Action)" path="/param[@name='action']"/></param>
    /// <param name="state">The parameter of the method that this delegate encapsulates.</param>
    void Post<T>(Action<T> action, T state);

    /// <inheritdoc cref="Post(Action)"/>
    /// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the return value of the method that this delegate encapsulates.
    /// </typeparam>
    /// <param name="action"><inheritdoc cref="Post(Action)" path="/param[@name='action']"/></param>
    /// <param name="state">The parameter of the method that this delegate encapsulates.</param>
    void Post<T, TResult>(Func<T, TResult> action, T state);

    /// <inheritdoc cref="Post(Action)"/>
    /// <typeparam name="TResult">
    /// The type of the return value of the method that this delegate encapsulates.
    /// </typeparam>
    /// <param name="action"><inheritdoc cref="Post(Action)" path="/param[@name='action']"/></param>
    void Post<TResult>(Func<TResult> action);
}
