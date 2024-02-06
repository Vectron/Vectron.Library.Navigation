using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vectron.Library.Navigation.Builder;

namespace Vectron.Library.Navigation.Wpf;

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
    public static NavigationBuilder AddWpfNavigation(this IServiceCollection services)
    {
        services.TryAddSingleton<INavigationWindowManager, WpfWindowManager>();
        return services.AddNavigation();
    }
}
