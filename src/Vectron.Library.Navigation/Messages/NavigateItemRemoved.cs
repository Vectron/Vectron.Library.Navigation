namespace Vectron.Library.Navigation.Messages;

/// <summary>
/// A message that a navigation item is removed.
/// </summary>
public sealed record class NavigateItemRemoved(INavigationItem NavigationItem);
