using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Vectron.Library.Navigation.Builder;
using Vectron.Library.Navigation.Messages;
using Vectron.Library.Navigation.Threading;

namespace Vectron.Library.Navigation;

/// <summary>
/// The view model for navigation.
/// </summary>
public sealed partial class NavigationViewModel
    : ObservableRecipient,
    IRecipient<NavigateToMessage>,
    IRecipient<NavigateByCaptionMessage>,
    IRecipient<NavigateBackMessage>,
    IRecipient<NavigateItemRemoved>
{
    private readonly INavigationHistoryHandler history;
    private readonly INavigationItem rootNavigationItem;
    private readonly IWindowManager windowManager;

    [ObservableProperty]
    private INavigationItem? currentNavigationItem;

    [ObservableProperty]
    private object? currentViewModel;

    [ObservableProperty]
    private NavigationItemViewModel root;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
    /// </summary>
    /// <param name="navigationItemFactory">A <see cref="INavigationItemFactory"/>.</param>
    /// <param name="messenger">A <see cref="IMessenger"/>.</param>
    /// <param name="history">A <see cref="INavigationHistoryHandler"/>.</param>
    /// <param name="uiSynchronizationContext">The <see cref="IUiSynchronizationContext"/>.</param>
    /// <param name="windowManager">The <see cref="IWindowManager"/>.</param>
    public NavigationViewModel(
        INavigationItemFactory navigationItemFactory,
        IMessenger messenger,
        INavigationHistoryHandler history,
        IUiSynchronizationContext uiSynchronizationContext,
        IWindowManager windowManager)
        : base(messenger)
    {
        this.history = history;
        this.windowManager = windowManager;
        rootNavigationItem = navigationItemFactory.CreateNavigationItem(NavigationItemOptions.RootNavigationItemId)
            ?? throw new InvalidOperationException("No navigation has been set up");
        root = new NavigationItemViewModel(rootNavigationItem, messenger, uiSynchronizationContext);
        messenger.RegisterAll(this);

        var first = Root.Children.FirstOrDefault();
        first?.NavigateToCommand.Execute(parameter: false);
    }

    /// <inheritdoc/>
    public void Receive(NavigateToMessage message)
        => NavigateTo(x => x.Id == message.Id);

    /// <inheritdoc/>
    public void Receive(NavigateBackMessage message)
        => NavigateBack();

    /// <inheritdoc/>
    public void Receive(NavigateByCaptionMessage message)
        => NavigateTo(x => string.Equals(x.Caption, message.Caption, StringComparison.OrdinalIgnoreCase));

    /// <inheritdoc/>
    public void Receive(NavigateItemRemoved message)
    {
        history.Remove(message.NavigationItem);
        if (CurrentNavigationItem == message.NavigationItem)
        {
            CurrentNavigationItem = null;
            CurrentViewModel = null;
            NavigateBack();
        }
    }

    private static IEnumerable<INavigationItem> IterateAllChildren(INavigationItem navigationItem)
    {
        yield return navigationItem;
        var providerChildren = navigationItem.ChildrenProvider?.NavigationItems ?? Enumerable.Empty<INavigationItem>();
        var selector = navigationItem.Children.Concat(providerChildren).SelectMany(IterateAllChildren);
        foreach (var child in selector)
        {
            yield return child;
        }
    }

    private void NavigateBack()
    {
        var previousPage = history.PreviousEntry();
        NavigateTo(previousPage);
    }

    private void NavigateTo(INavigationItem? target)
    {
        if (target == null)
        {
            return;
        }

        if (CurrentNavigationItem != null)
        {
            history.AddEntry(CurrentNavigationItem);
        }

        CurrentNavigationItem = target;
        CurrentViewModel = CurrentNavigationItem?.ViewModelFactory.Invoke();
    }

    private void NavigateTo(Func<INavigationItem, bool> predicate)
    {
        var foundItem = IterateAllChildren(rootNavigationItem)
            .FirstOrDefault(predicate);
        NavigateTo(foundItem);
    }

    [RelayCommand]
    private void PopOut()
    {
        if (CurrentViewModel == null)
        {
            return;
        }

        windowManager.OpenWindow(CurrentViewModel);
        NavigateBack();
    }
}
