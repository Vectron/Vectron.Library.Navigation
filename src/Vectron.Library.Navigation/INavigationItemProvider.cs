namespace Vectron.Library.Navigation;

/// <summary>
/// A class that provides a list with navigation items to display.
/// </summary>
public interface INavigationItemProvider
{
    /// <summary>
    /// Gets a <see cref="IEnumerable{T}"/> containing the child <see cref="INavigationItem"/>.
    /// </summary>
    IEnumerable<INavigationItem> NavigationItems
    {
        get;
    }
}
