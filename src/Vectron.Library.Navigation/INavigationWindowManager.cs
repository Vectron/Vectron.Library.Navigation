namespace Vectron.Library.Navigation;

/// <summary>
/// A manager for opening windows.
/// </summary>
public interface INavigationWindowManager
{
    /// <summary>
    /// Open a new window with the given data context.
    /// </summary>
    /// <param name="viewModel">The viewModel to show in the window.</param>
    void OpenWindow(object viewModel);
}
