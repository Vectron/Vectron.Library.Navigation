namespace Vectron.Library.Navigation.Internal;

/// <summary>
/// A implementation of <see cref="INavigationHistoryHandler"/>.
/// </summary>
internal sealed class NavigationHistoryHandler : INavigationHistoryHandler
{
    private const int MaxHistory = 10;
    private readonly LinkedList<INavigationItem> history = new();
    private LinkedListNode<INavigationItem>? current;

    /// <inheritdoc/>
    public void AddEntry(INavigationItem entry)
    {
        if (entry == null)
        {
            return;
        }

        if (history.Count > MaxHistory)
        {
            history.RemoveFirst();
        }

        if (history.Last != null
            && history.Last.Equals(entry))
        {
            return;
        }

        _ = history.AddLast(entry);
        current = history.Last;
    }

    /// <inheritdoc/>
    public INavigationItem? NextEntry()
    {
        if (current == null)
        {
            return default;
        }

        current = current.Next;
        if (current == null)
        {
            return default;
        }

        return current.Value;
    }

    /// <inheritdoc/>
    public INavigationItem? PreviousEntry()
    {
        if (current == null)
        {
            current = history.Last;
            if (current == null)
            {
                return default;
            }

            return current.Value ?? default;
        }

        if (current == history.First)
        {
            return current.Value;
        }

        var previousItem = current.Previous;
        if (previousItem == null)
        {
            return default;
        }

        current = previousItem;
        return current.Value;
    }

    /// <inheritdoc/>
    public void Remove(INavigationItem entry)
    {
        while (history.Remove(entry))
        {
        }

        if (entry != null
            && current != null
            && entry.Equals(current.Value))
        {
            current = history.Last;
        }
    }
}
