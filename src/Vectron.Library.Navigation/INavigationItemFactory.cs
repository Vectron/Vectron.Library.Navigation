namespace Vectron.Library.Navigation;

/// <summary>
/// Represents a class for creating <see cref="INavigationItem"/>.
/// </summary>
public interface INavigationItemFactory
{
    /// <summary>
    /// Create a <see cref="INavigationItem"/> with the given Id.
    /// </summary>
    /// <param name="id">The id of the item.</param>
    /// <returns>The constructed <see cref="INavigationItem"/>, otherwise <see langword="null"/>.</returns>
    INavigationItem CreateNavigationItem(Guid id);

    /// <summary>
    /// Create a <see cref="INavigationItem"/> with the given Id.
    /// </summary>
    /// <param name="id">The id of the item.</param>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to resolve items.</param>
    /// <returns>The constructed <see cref="INavigationItem"/>, otherwise <see langword="null"/>.</returns>
    INavigationItem CreateNavigationItem(Guid id, IServiceProvider serviceProvider);
}
