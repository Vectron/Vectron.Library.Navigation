using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Vectron.Library.Navigation.Builder;

/// <summary>
/// A builder for creating the navigation menu.
/// </summary>
/// <param name="services">The service provider to register items too.</param>
/// <param name="id">The unique id for this item.</param>
/// <param name="isRoot">Indicates weather this is the root item.</param>
public sealed class NavigationBuilder(IServiceCollection services, Guid id, bool isRoot)
{
    private readonly string optionKey = id.ToString();

    /// <summary>
    /// Gets the id of this child.
    /// </summary>
    public Guid Id { get; } = id;

    /// <summary>
    /// Add a child navigation item.
    /// </summary>
    /// <returns>A reference to the <see cref="NavigationBuilder"/> for the child.</returns>
    public NavigationBuilder AddChild() => AddChild(Guid.NewGuid());

    /// <summary>
    /// Add a child navigation item.
    /// </summary>
    /// <param name="id">The id of the child.</param>
    /// <returns>A reference to the <see cref="NavigationBuilder"/> for the child.</returns>
    public NavigationBuilder AddChild(Guid id)
    {
        var childBuilder = new NavigationBuilder(services, id, isRoot: false);
        _ = services.Configure<NavigationItemOptions>(optionKey, o => o.Children.Add(id));
        return childBuilder;
    }

    /// <summary>
    /// Add a child navigation item.
    /// </summary>
    /// <param name="setupChild">A function to setup the child navigation item.</param>
    /// <returns>A reference to the <see cref="NavigationBuilder"/> for the child.</returns>
    public NavigationBuilder AddChild(Action<NavigationBuilder> setupChild) => AddChild(Guid.NewGuid(), setupChild);

    /// <summary>
    /// Add a child navigation item.
    /// </summary>
    /// <param name="id">The id of the child.</param>
    /// <param name="setupChild">A function to setup the child navigation item.</param>
    /// <returns>A reference to the <see cref="NavigationBuilder"/> for the child.</returns>
    public NavigationBuilder AddChild(Guid id, Action<NavigationBuilder> setupChild)
    {
        var childBuilder = new NavigationBuilder(services, id, isRoot: false);
        _ = services.Configure<NavigationItemOptions>(optionKey, o => o.Children.Add(id));
        setupChild.Invoke(childBuilder);
        return this;
    }

    /// <summary>
    /// Set the auto navigate for this item..
    /// </summary>
    /// <param name="autoNavigate"><see langword="true"/> when the items gets navigated to on adding.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public NavigationBuilder AutoNavigate(bool autoNavigate = true)
    {
        if (!isRoot)
        {
            _ = services.Configure<NavigationItemOptions>(optionKey, o => o.AutoNavigate = autoNavigate);
            return this;
        }

        var childBuilder = AddChild();
        return childBuilder.AutoNavigate(autoNavigate);
    }

    /// <summary>
    /// The caption for the button.
    /// </summary>
    /// <param name="caption">The caption text to display.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public NavigationBuilder Caption(string caption) => Caption(_ => caption);

    /// <summary>
    /// Add a function to generate the button caption.
    /// </summary>
    /// <param name="captionProvider">A <see cref="Func{T, TResult}"/> for creating the caption text.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public NavigationBuilder Caption(Func<IServiceProvider, string> captionProvider)
    {
        if (!isRoot)
        {
            _ = services.Configure<NavigationItemOptions>(optionKey, o => o.CaptionProvider = captionProvider);
            return this;
        }

        var childBuilder = AddChild();
        return childBuilder.Caption(captionProvider);
    }

    /// <summary>
    /// Set the view model to use when displaying the content.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public NavigationBuilder Content<TViewModel>()
        where TViewModel : class
    {
        if (!isRoot)
        {
            _ = services.Configure<NavigationItemOptions>(optionKey, o => o.Content = typeof(TViewModel));
            _ = services.AddScoped<TViewModel>();
            return this;
        }

        var childBuilder = AddChild();
        return childBuilder.Content<TViewModel>();
    }

    /// <summary>
    /// Set the provider that can generate items at runtime.
    /// </summary>
    /// <typeparam name="TProvider">The type of provider.</typeparam>
    /// <param name="id">THe id to look up options.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public NavigationBuilder Provider<TProvider>(Guid id)
        where TProvider : class, INavigationItemProvider
    {
        _ = services.Configure<NavigationItemOptions>(optionKey, o => o.RuntimeItemsProvider = typeof(TProvider));
        services.TryAddSingleton<TProvider>();
        return new NavigationBuilder(services, id, isRoot: false);
    }
}
