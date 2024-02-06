using CommunityToolkit.Mvvm.ComponentModel;

namespace Vectron.Library.Navigation.Wpf.Demo;

/// <summary>
/// The view model for the main window.
/// </summary>
/// <param name="navigationViewModel">The <see cref="NavigationViewModel"/>.</param>
public sealed partial class MainWindowViewModel(NavigationViewModel navigationViewModel) : ObservableObject
{
    [ObservableProperty]
    private NavigationViewModel navigationViewModel = navigationViewModel;
}
