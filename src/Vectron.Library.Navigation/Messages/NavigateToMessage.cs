namespace Vectron.Library.Navigation.Messages;

/// <summary>
/// A Message to navigate to an other page.
/// </summary>
/// <param name="Id">Target page.</param>
public sealed record class NavigateToMessage(Guid Id);
