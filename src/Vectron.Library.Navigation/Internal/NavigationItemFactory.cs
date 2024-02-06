using Microsoft.Extensions.Options;
using Vectron.Library.Navigation.Builder;

namespace Vectron.Library.Navigation.Internal;

/// <summary>
/// Implementation of <see cref="INavigationItemFactory"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NavigationItemFactory"/> class.
/// </remarks>
/// <param name="optionsMonitor">The <see cref="IOptionsMonitor{TOptions}"/> for getting <see cref="NavigationItemOptions"/>.</param>
/// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
internal sealed partial class NavigationItemFactory(
    IOptionsMonitor<NavigationItemOptions> optionsMonitor,
    IServiceProvider serviceProvider)
    : INavigationItemFactory
{
    /// <inheritdoc/>
    public INavigationItem CreateNavigationItem(Guid id)
        => CreateNavigationItem(id, serviceProvider);

    /// <inheritdoc/>
    public INavigationItem CreateNavigationItem(Guid id, IServiceProvider provider)
    {
        var options = optionsMonitor.Get(id.ToString());
        var caption = options.CaptionProvider?.Invoke(provider) ?? string.Empty;
        var children = options.Children.Select(CreateNavigationItem).ToArray();
        var navigationItemProvider = options.RuntimeItemsProvider != null
            ? provider.GetService(options.RuntimeItemsProvider) as INavigationItemProvider
            : null;

        var navigationItem = new NavigationItem(
            Guid.NewGuid(),
            caption,
            () => CreateViewModel(options.Content),
            options.AutoNavigate,
            children,
            navigationItemProvider);

        return navigationItem;

        object? CreateViewModel(Type? content)
        {
            if (content == null)
            {
                return null;
            }

            try
            {
                return provider.GetService(content);
            }
            catch (ObjectDisposedException)
            {
                // The service provider might already be Disposed, catch it here so that we don't blow up elsewhere
                return null;
            }
        }
    }
}
