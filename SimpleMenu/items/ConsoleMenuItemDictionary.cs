using System.Collections;

namespace SimpleMenu.items;

public sealed class ConsoleMenuItemDictionary<TV> :
    IMenuItemDictionary<string, TV>
{
    private readonly IDictionary<string, TV> dict;

    public ConsoleMenuItemDictionary(IDictionary<string, TV>? dict = default)
    {
        this.dict = dict ?? new Dictionary<string, TV>();
    }

    public void Add(IDictionary<string, TV> items)
    {
        foreach (var item in items)
            if(!Contains(item))
                Add(item);
    }

    public IEnumerator<KeyValuePair<string, TV>> GetEnumerator()
    {
        return dict.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dict.GetEnumerator();
    }

    public void Add(KeyValuePair<string, TV> item)
    {
        dict.Add(item);
    }

    public void Clear()
    {
        dict.Clear();
    }

    public bool Contains(KeyValuePair<string, TV> item)
    {
        return dict.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, TV>[] array, int arrayIndex)
    {
        dict.CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<string, TV> item)
    {
        return dict.Remove(item);
    }
    
    public int Count => dict.Count;

    public bool IsReadOnly => dict.IsReadOnly;
    
    public void Add(string key, TV value)
    {
        dict.Add(key, value);
    }

    public bool ContainsKey(string key)
    {
        return dict.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        return dict.Remove(key);
    }

    public bool TryGetValue(string key, out TV value)
    {
        return dict.TryGetValue(key, out value!);
    }

    public TV this[string key]
    {
        get => dict[key];
        set => dict[key] = value;
    }

    public ICollection<string> Keys => dict.Keys;

    public ICollection<TV> Values => dict.Values;
}