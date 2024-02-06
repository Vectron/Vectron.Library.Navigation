namespace Vectron.Library.Navigation.Messages;

/// <summary>
/// A Message to navigate to an other page by the caption name.
/// </summary>
/// <param name="Caption">The caption to use to find the target.</param>
public sealed record class NavigateByCaptionMessage(string Caption);
