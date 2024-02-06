namespace Vectron.Library.Navigation;

/// <summary>
/// Represents a navigation target.
/// </summary>
public interface INavigationItem
{
    /// <summary>
    /// Gets a value indicating whether this item should be navigated to when added to the view.
    /// </summary>
    bool AutoNavigate
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the caption of the item.
    /// </summary>
    string Caption
    {
        get;
        init;
    }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> of child <see cref="INavigationItem"/>s.
    /// </summary>
    IEnumerable<INavigationItem> Children
    {
        get;
    }

    /// <summary>
    /// Gets a provider that supplies more children.
    /// </summary>
    INavigationItemProvider? ChildrenProvider
    {
        get;
        init;
    }

    /// <summary>
    /// Gets the Unique id of this item.
    /// </summary>
    Guid Id
    {
        get;
        init;
    }

    /// <summary>
    /// Gets a factory for creating the view model.
    /// </summary>
    Func<object?> ViewModelFactory
    {
        get;
        init;
    }
}
