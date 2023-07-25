namespace DU4;

public class ItemEventArgs<T> : EventArgs
{
    public T Item { get; }
    public ItemEventArgs(T item)
    {
        Item = item;
    }
}