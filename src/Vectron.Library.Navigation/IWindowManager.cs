namespace Vectron.Library.Navigation;

/// <summary>
/// A manager for opening windows.
/// </summary>
public interface IWindowManager
{
    /// <summary>
    /// Open a new window with the given data context.
    /// </summary>
    /// <param name="dataContext">The data context to set on the window.</param>
    void OpenWindow(object dataContext);
}
