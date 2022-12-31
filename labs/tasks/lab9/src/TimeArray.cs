using System.Collections;

namespace labs.lab9.src;

public class TimeArray :
    IList<Time>
{
    private const int DefaultCapacity = 4;
    
    private Time[] times;

    public TimeArray() :
        this(DefaultCapacity)
    {}

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
    }

    public IEnumerator<Time> GetEnumerator() => new Enumerator(times);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Time item) => Insert(times.Length, item);

    public void Clear() => Array.Clear(times);

    public bool Contains(Time item) => IndexOf(item) != -1;

    public void CopyTo(Time[] array, int arrayIndex) =>
        Array.Copy(times, 0, array, arrayIndex, times.Length);

    public bool Remove(Time item)
    {
        int index;

        if ((index = IndexOf(item)) == -1)
            return false;

        RemoveAt(index);
        return true;
    }

    public int Count => times.Length;
    
    public bool IsReadOnly => false;

    public int IndexOf(Time item) => Array.IndexOf(times, item, 0, times.Length);

    public void Insert(int index, Time item)
    {
        int newSize = times.Length + 1;
        
        ThrowableInRange(0, index, newSize);

        Resize((uint)newSize);
        
        Array.Copy(times, index, times, index + 1, newSize - index - 1);

        times[index] = item;
    }

    public void RemoveAt(int index)
    {
        ThrowableInRange(0, index, times.Length);

        int size = times.Length;
        
        Array.Copy(times, index + 1, times, index, size - 1);
        
        Resize((uint)(size - 1));
    }

    public Time this[int index]
    {
        get
        {
            ThrowableInRange(0, index, times.Length);
            return times[index];
        }

        set
        {
            ThrowableInRange(0, index, times.Length);
            times[index] = value;
        }
    }

    public class Enumerator : IEnumerator<Time>
    {
        private readonly Time[] array;

        private Time? current;

        private int index;
        
        internal Enumerator(Time[] array)
        {
            this.array = array;
            index = 0;
            current = default;
        }
        
        public bool MoveNext()
        {
            if (index >= array.Length)
            {
                current = default;
                return false;
            }
            
            current = array[index++];
            
            return true;
        }

        public void Reset()
        {
            index = 0;
            current = default;
        }

        public Time Current => current!;
        
        object IEnumerator.Current
        {
            get
            {
                ThrowableInRange(0, index, array.Length);
                
                return Current;
            }
        }

        public void Dispose()
        { }
    }

    private void Resize(uint size)
    {
        Array.Resize(ref times, (int)size);
    }
    
    private static void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }
}