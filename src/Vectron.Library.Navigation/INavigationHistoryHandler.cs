namespace Vectron.Library.Navigation;

/// <summary>
/// This class handles the history.
/// </summary>
public interface INavigationHistoryHandler
{
    /// <summary>
    /// Add a new item to the history.
    /// </summary>
    /// <param name="entry">The entry to add.</param>
    void AddEntry(INavigationItem entry);

    /// <summary>
    /// Gets the next history item.
    /// </summary>
    /// <returns>The next history item when available.</returns>
    INavigationItem? NextEntry();

    /// <summary>
    /// Gets the previous history item.
    /// </summary>
    /// <returns>The previous history item when available.</returns>
    INavigationItem? PreviousEntry();

    /// <summary>
    /// Remove an entry from the history.
    /// </summary>
    /// <param name="entry">The entry to remove.</param>
    void Remove(INavigationItem entry);
}
