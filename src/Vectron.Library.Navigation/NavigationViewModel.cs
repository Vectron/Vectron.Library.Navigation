using System.Reflection;
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
    private readonly INavigationWindowManager? windowManager;

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
    /// <param name="windowManager">The <see cref="INavigationWindowManager"/>.</param>
    public NavigationViewModel(
        INavigationItemFactory navigationItemFactory,
        IMessenger messenger,
        INavigationHistoryHandler history,
        IUiSynchronizationContext uiSynchronizationContext,
        INavigationWindowManager? windowManager = null)
        : base(messenger)
    {
        this.history = history;
        this.windowManager = windowManager;
        rootNavigationItem = navigationItemFactory.CreateNavigationItem(NavigationItemOptions.RootNavigationItemId)
            ?? throw new InvalidOperationException("No navigation has been set up");
        root = new NavigationItemViewModel(rootNavigationItem, messenger, uiSynchronizationContext);

        IsActive = true;

        var first = Root.Children.FirstOrDefault();
        first?.NavigateToCommand.Execute(parameter: false);
    }

    private bool CanExecutePopOut => windowManager != null;

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
            SetIsActive(CurrentViewModel, newValue: false);
            CurrentViewModel = null;
            NavigateBack();
        }
    }

    private static IEnumerable<INavigationItem> IterateAllChildren(INavigationItem navigationItem)
    {
        yield return navigationItem;
        var providerChildren = navigationItem.ChildrenProvider?.NavigationItems ?? [];
        var selector = navigationItem.Children.Concat(providerChildren).SelectMany(IterateAllChildren);
        foreach (var child in selector)
        {
            yield return child;
        }
    }

    private static void SetIsActive(object? viewModel, bool newValue)
    {
        if (viewModel == null)
        {
            return;
        }

        if (viewModel is ObservableRecipient recipient)
        {
            recipient.IsActive = newValue;
            return;
        }

        var viewModelType = viewModel.GetType();
        var recepientAttribute = viewModelType.GetCustomAttribute<ObservableRecipientAttribute>(inherit: true);
        if (recepientAttribute == null)
        {
            return;
        }

        var isActiveProperty = viewModelType.GetProperty(nameof(IsActive));
        isActiveProperty?.SetValue(viewModel, newValue);
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

        SetIsActive(CurrentViewModel, newValue: false);
        CurrentNavigationItem = target;
        CurrentViewModel = CurrentNavigationItem?.ViewModelFactory.Invoke();
        SetIsActive(CurrentViewModel, newValue: true);
    }

    private void NavigateTo(Func<INavigationItem, bool> predicate)
    {
        var foundItem = IterateAllChildren(rootNavigationItem)
            .FirstOrDefault(predicate);
        NavigateTo(foundItem);
    }

    [RelayCommand(CanExecute = nameof(CanExecutePopOut))]
    private void PopOut()
    {
        if (windowManager == null
            || CurrentViewModel == null
            || CurrentNavigationItem == null)
        {
            return;
        }

        windowManager.OpenWindow(CurrentViewModel, CurrentNavigationItem.Caption);
        NavigateBack();
    }
}
