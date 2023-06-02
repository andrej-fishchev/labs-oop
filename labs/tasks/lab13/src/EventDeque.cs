using System.Collections;
using labs.shared.data.abstracts;
using labs.tasks.lab13.src.delegates;

namespace labs.tasks.lab13.src;

public class EventDeque<T> : ICollection<T>, IDeque<T>
{
    public string Id { get; }

    private Deque<T> container;

    public event EventDequeActionDelegate? Subscribers;

    public EventDeque()
    {
        Id = Guid.NewGuid().ToString();
        container = new Deque<T>();
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("GetEnumerator"));

        return container.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("IEnumerable.GetEnumerator"));

        return GetEnumerator();
    }

    public void Add(T item)
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("Add", item));

        var prev = container.Count;
        
        container.Add(item);
        
        OnCountChanged(prev, container.Count);
    }

    public void Clear()
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("Clear"));

        var prev = container.Count;
        
        container.Clear();
        
        OnCountChanged(prev, container.Count);
    }

    public bool Contains(T item)
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("Contains", item));

        return container.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("CopyTo", (array, arrayIndex)));
        
        container.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var prev = container.Count;
        
        Subscribers?.Invoke(this, new EventDequeActionArgs("Remove", item));

        bool success = container.Remove(item);
        
        OnCountChanged(prev, container.Count);
        
        return success;
    }

    public int Count
    {
        get
        {
            Subscribers?.Invoke(this, new EventDequeActionArgs("Count"));

            return container.Count;
        }
    }

    public bool IsReadOnly
    {
        get
        {
            Subscribers?.Invoke(this, new EventDequeActionArgs("IReadOnly"));

            return container.IsReadOnly;
        }
    }

    public T Back()
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("GetBack"));

        int prev = container.Count;
        
        var data = container.Back();
        
        OnCountChanged(prev, container.Count);
        
        return data;
    }

    public void Back(T value)
    {
        var prev = container.Count;
        
        Subscribers?.Invoke(this, new EventDequeActionArgs("SetBack", value));
        
        container.Back(value);
        
        OnCountChanged(prev, container.Count);
    }

    public T Front()
    {
        Subscribers?.Invoke(this, new EventDequeActionArgs("GetFront"));
       
        var prev = container.Count;
        
        var data = container.Front();
        
        OnCountChanged(prev, container.Count);
        
        return data;
    }

    public void Front(T value)
    {
        var prev = container.Count;
        
        Subscribers?.Invoke(this, new EventDequeActionArgs("SetFront", value));
        
        container.Front(value);
        
        OnCountChanged(prev, container.Count);
    }

    public void RemoveAt(int index)
    {
        var prev = container.Count;
        
        Subscribers?.Invoke(this, new EventDequeActionArgs("RemoveAt", index));
        
        container.RemoveAt(index);

        OnCountChanged(prev, container.Count);
    }

    public T this[int index]
    {
        get
        {
            Subscribers?.Invoke(this, new EventDequeActionArgs("Get[]", index));
            return container[index];
        }

        set
        {
            Subscribers?.Invoke(this, new EventDequeActionArgs("Set[]", (index, value)));

            var prev = container[index]!.GetHashCode();
            
            container[index] = value;

            OnRefChanged(index, prev, container[index]!.GetHashCode());
        }
    }

    private void OnCountChanged(int prev, int now)
    {
        if(prev != now)
            Subscribers?.Invoke(this, new EventDequeActionArgs("OnCountChanged", (prev, now)));
    }
        
    
    private void OnRefChanged(int index, int prevHash, int nowHash)
    {
        if(prevHash != nowHash)
            Subscribers?.Invoke(this, new EventDequeActionArgs("OnRefChanged", (index, prevHash, nowHash)));
    }
}