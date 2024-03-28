using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.Messaging;
using Vectron.Library.Navigation.Messages;

namespace Vectron.Library.Navigation;

/// <summary>
/// Base helper implementation of <see cref="INavigationItemProvider"/>.
/// </summary>
internal abstract class NavigationItemProvider : INavigationItemProvider
{
    private readonly IMessenger messenger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationItemProvider"/> class.
    /// </summary>
    protected NavigationItemProvider()
        : this(WeakReferenceMessenger.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationItemProvider"/> class.
    /// </summary>
    /// <param name="messenger">The <see cref="IMessenger"/> to use.</param>
    protected NavigationItemProvider(IMessenger messenger)
    {
        this.messenger = messenger;
        NavigationItemsCollection.CollectionChanged += NavigationItems_CollectionChanged;
    }

    /// <inheritdoc/>
    IEnumerable<INavigationItem> INavigationItemProvider.NavigationItems
        => new ReadOnlyObservableCollection<INavigationItem>(NavigationItemsCollection);

    /// <summary>
    /// Gets all active <see cref="INavigationItem"/>.
    /// </summary>
    protected ObservableCollection<INavigationItem> NavigationItemsCollection { get; } = [];

    private void NavigationItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Remove:
            case NotifyCollectionChangedAction.Replace:
                SendItemRemoved(e.OldItems);
                break;

            case NotifyCollectionChangedAction.Reset:
            case NotifyCollectionChangedAction.Add:
            case NotifyCollectionChangedAction.Move:
            default:
                break;
        }
    }

    private void SendItemRemoved(IList? items)
    {
        if (items == null)
        {
            return;
        }

        foreach (var item in items)
        {
            if (item is not INavigationItem navigationItem)
            {
                continue;
            }

            _ = messenger.Send(new NavigateItemRemoved(navigationItem));
        }
    }
}
