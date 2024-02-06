using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Vectron.Library.Navigation.Messages;
using Vectron.Library.Navigation.Threading;

namespace Vectron.Library.Navigation;

/// <summary>
/// A view model for the <see cref="INavigationItem"/>.
/// </summary>
public sealed partial class NavigationItemViewModel : ObservableObject
{
    private readonly ObservableCollection<NavigationItemViewModel> children;
    private readonly IMessenger messenger;
    private readonly INavigationItem navigationItem;
    private readonly IUiSynchronizationContext uiSynchronizationContext;

    [ObservableProperty]
    private string caption;

    [ObservableProperty]
    private Guid id;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationItemViewModel"/> class.
    /// </summary>
    /// <param name="navigationItem">The <see cref="INavigationItem"/> to view.</param>
    /// <param name="messenger">The <see cref="IMessenger"/>.</param>
    /// <param name="uiSynchronizationContext">The <see cref="IUiSynchronizationContext"/>.</param>
    public NavigationItemViewModel(INavigationItem navigationItem, IMessenger messenger, IUiSynchronizationContext uiSynchronizationContext)
    {
        this.navigationItem = navigationItem;
        this.messenger = messenger;
        this.uiSynchronizationContext = uiSynchronizationContext;
        caption = navigationItem.Caption;
        id = navigationItem.Id;

        var selector = navigationItem.Children;
        if (navigationItem.ChildrenProvider != null)
        {
            selector = selector.Concat(navigationItem.ChildrenProvider.NavigationItems);
            if (navigationItem.ChildrenProvider.NavigationItems is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;
            }
        }

        if (navigationItem.AutoNavigate)
        {
            NavigateTo();
        }

        children = new(selector.Select(CreateViewModel));
    }

    /// <summary>
    /// Gets all children.
    /// </summary>
    public ReadOnlyObservableCollection<NavigationItemViewModel> Children
        => new(children);

    /// <summary>
    /// Add all children from the given <see cref="IEnumerable"/> to the <see cref="Children"/>.
    /// </summary>
    /// <param name="items">The <see cref="IEnumerable"/> of items to add.</param>
    private void AddChildren(IEnumerable? items)
    {
        if (items == null)
        {
            return;
        }

        uiSynchronizationContext.Post(() =>
        {
            foreach (INavigationItem child in items)
            {
                children.Add(CreateViewModel(child));
            }
        });
    }

    private NavigationItemViewModel CreateViewModel(INavigationItem item) => new(item, messenger, uiSynchronizationContext);

    [RelayCommand]
    private void NavigateTo()
    {
        var message = new NavigateToMessage(navigationItem.Id);
        _ = messenger.Send(message);
    }

    private void NotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                AddChildren(e.NewItems);
                break;

            case NotifyCollectionChangedAction.Remove:
                RemoveChildren(e.OldItems);
                break;

            case NotifyCollectionChangedAction.Replace:
                AddChildren(e.NewItems);
                RemoveChildren(e.OldItems);
                break;

            case NotifyCollectionChangedAction.Reset:
                break;

            case NotifyCollectionChangedAction.Move:
            default:
                break;
        }
    }

    /// <summary>
    /// Remove all children from the given <see cref="IEnumerable"/> to the <see cref="Children"/>.
    /// </summary>
    /// <param name="items">The <see cref="IEnumerable"/> of items to add.</param>
    private void RemoveChildren(IEnumerable? items)
    {
        if (items == null)
        {
            return;
        }

        uiSynchronizationContext.Post(() =>
        {
            foreach (INavigationItem child in items)
            {
                var itemToRemove = children.FirstOrDefault(x => x.navigationItem.Id == child.Id);
                if (itemToRemove == null)
                {
                    continue;
                }

                _ = children.Remove(itemToRemove);
            }
        });
    }
}
