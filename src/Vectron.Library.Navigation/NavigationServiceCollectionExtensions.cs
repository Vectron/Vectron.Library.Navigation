using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vectron.Library.Navigation.Builder;
using Vectron.Library.Navigation.Internal;

namespace Vectron.Library.Navigation;

/// <summary>
/// Extension methods for adding services to the <see cref="IServiceCollection"/>.
/// </summary>
public static class NavigationServiceCollectionExtensions
{
    /// <summary>
    /// Add navigation to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add navigation to.</param>
    /// <returns>The root <see cref="NavigationBuilder"/>.</returns>
    public static NavigationBuilder AddNavigation(this IServiceCollection services)
    {
        services.TryAddSingleton<IWindowManager, DefaultWindowManager>();
        services.TryAddSingleton<INavigationHistoryHandler, NavigationHistoryHandler>();
        services.TryAddTransient<INavigationItemFactory, NavigationItemFactory>();
        services.TryAddTransient<NavigationViewModel>();
        return new NavigationBuilder(services, NavigationItemOptions.RootNavigationItemId, isRoot: true);
    }
}
