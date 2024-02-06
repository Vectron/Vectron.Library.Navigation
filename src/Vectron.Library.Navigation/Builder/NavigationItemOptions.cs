namespace Vectron.Library.Navigation.Builder;

/// <summary>
/// Options that define a navigation item.
/// </summary>
internal sealed class NavigationItemOptions
{
    /// <summary>
    /// The id of the root navigation item.
    /// </summary>
    public static readonly Guid RootNavigationItemId = new("381c857d-9357-4209-9d21-7a6d1142ac5b");

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationItemOptions"/> class.
    /// </summary>
    public NavigationItemOptions() => Children = new List<Guid>();

    /// <summary>
    /// Gets or sets a value indicating whether a value indicating whether this item automatically gets navigated too.
    /// </summary>
    public bool AutoNavigate
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the caption to show on the button.
    /// </summary>
    public Func<IServiceProvider, string>? CaptionProvider
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a <see cref="IList{T}"/> with all the ids of the child navigation items.
    /// </summary>
    public IList<Guid> Children
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the type of the view model to show.
    /// </summary>
    public Type? Content
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the type of the view model to show.
    /// </summary>
    public Type? RuntimeItemsProvider
    {
        get;
        set;
    }
}
