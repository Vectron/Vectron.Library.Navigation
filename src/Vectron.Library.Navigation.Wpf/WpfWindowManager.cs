using System.Windows;

namespace Vectron.Library.Navigation.Wpf;

/// <summary>
/// Wpf implementation of <see cref="INavigationWindowManager"/>.
/// </summary>
internal sealed class WpfWindowManager : INavigationWindowManager
{
    private static readonly object CollectionLock = new();
    private static readonly List<NavigationItemWindow> OpenWindows = [];

    /// <inheritdoc/>
    public void OpenWindow(object dataContext, string caption)
    {
        var window = new NavigationItemWindow()
        {
            DataContext = dataContext,
            ShowActivated = true,
            Title = caption,
            Owner = Application.Current.MainWindow,
        };

        window.Closed += Window_Closed;
        lock (CollectionLock)
        {
            OpenWindows.Add(window);
        }

        window.Show();
    }

    /// <inheritdoc/>
    public void OpenWindow(object viewModel) => OpenWindow(viewModel, string.Empty);

    private void Window_Closed(object? sender, EventArgs e)
    {
        if (sender is not NavigationItemWindow window)
        {
            return;
        }

        window.Closed -= Window_Closed;
        lock (CollectionLock)
        {
            _ = OpenWindows.Remove(window);
        }
    }
}
