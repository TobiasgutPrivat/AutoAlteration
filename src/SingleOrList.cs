using System.Collections;

public class SList<T> : IEnumerable<T>
{
    private readonly List<T> _items;

    // Property to expose the internal list
    public List<T> Items => _items;

    // Default constructor
    public SList()
    {
        _items = [];
    }

    // Constructor accepting a single item
    public SList(T singleItem)
    {
        _items = [singleItem];
    }

    // Constructor accepting multiple items
    public SList(IEnumerable<T> multipleItems)
    {
        _items = new List<T>(multipleItems);
    }

    // Implicit conversion from single item
    public static implicit operator SList<T>(T singleItem)
    {
        return new SList<T>(singleItem);
    }

    // Implicit conversion from list
    public static implicit operator SList<T>(List<T> multipleItems)
    {
        return new SList<T>(multipleItems);
    }

    // Implicit conversion to List<T>
    public static implicit operator List<T>(SList<T> singleOrList)
    {
        return singleOrList.Items;
    }

    // Add method to support collection initializer syntax
    public void Add(T item)
    {
        _items.Add(item);
    }

    // Implement IEnumerable<T> to allow enumeration
    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }
}