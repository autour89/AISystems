using System;
namespace Laba1.Models;


public class PriorityQueue<T>
{
    private SortedSet<T> _set;

    public PriorityQueue(Func<T, T, int> comparer)
    {
        _set = new SortedSet<T>(Comparer<T>.Create((x, y) => comparer(y, x)));
    }

    public int Count => _set.Count;

    public void Enqueue(T item)
    {
        _set.Add(item);
    }

    public T Dequeue()
    {
        T? item = _set.Max;

        ArgumentNullException.ThrowIfNull(item);

        _set.Remove(item);

        return item;
    }
}