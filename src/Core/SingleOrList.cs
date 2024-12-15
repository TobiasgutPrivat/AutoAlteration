using System.Collections;

public class SList<T> : IEnumerable<T>
{
    private readonly List<T> _items;

    public List<T> Items => _items;

    public SList() => _items = [];

    public SList(T singleItem) => _items = [singleItem];

    public SList(IEnumerable<T> multipleItems) => _items = new List<T>(multipleItems);

    public static implicit operator SList<T>(T singleItem) =>
        new(singleItem);

    public static implicit operator SList<T>(List<T> multipleItems) =>
        new(multipleItems);

    public static implicit operator List<T>(SList<T> singleOrList) =>
        singleOrList.Items;

    public void Add(T item) => _items.Add(item);

    public IEnumerator<T> GetEnumerator() =>
        _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        _items.GetEnumerator();
}