namespace Vectron.Library.Navigation.Internal;

/// <summary>
/// Default implementation of <see cref="INavigationItem"/>.
/// </summary>
/// <param name="Id">The unique id of this item.</param>
/// <param name="Caption">The caption for this item.</param>
/// <param name="ViewModelFactory">A <see cref="Func{TResult}"/> to create a view mode.</param>
/// <param name="AutoNavigate">A value indicating if this item is automatically navigated too.</param>
/// <param name="Children">All children of this item.</param>
/// <param name="ChildrenProvider">A <see cref="INavigationItemProvider"/> that provides children for this item.</param>
internal record class NavigationItem(
    Guid Id,
    string Caption,
    Func<object?> ViewModelFactory,
    bool AutoNavigate,
    IEnumerable<INavigationItem> Children,
    INavigationItemProvider? ChildrenProvider = null) : INavigationItem;
