using System.Collections;
using IO.responses;

namespace labs.lab9.src;

public class TimeArray :
    IList<Time>
{
    private const int DefaultCapacity = 4;
    
    private Time[] times;

    private int size;
    
    public TimeArray()
    {
        times = Array.Empty<Time>();
    }

    public TimeArray(int capacity = 0)
    {
        ThrowableInRange(0, capacity, Int32.MaxValue);

        if (capacity == 0)
            times = Array.Empty<Time>();

        else times = new Time[capacity];
    }

    public TimeArray(Time[] data)
    {
        times = data ?? throw new ArgumentNullException();
        size = times.Length;
    }

    public IEnumerator<Time> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Time item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(Time item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(Time[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(Time item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    
    public bool IsReadOnly { get; }
    
    public int IndexOf(Time item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, Time item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public Time this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    
    private void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }
}