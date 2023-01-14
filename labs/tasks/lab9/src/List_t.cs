using System.Collections;

namespace labs.lab9.src;

public class List_t<T> :
    IList<T>
{
    private T[] m_array;
    
    private static readonly T[] Empty = Array.Empty<T>();

    public List_t(int capacity = 0)
    {
        ThrowableInRange(0, capacity, Int32.MaxValue);

        if (capacity == 0)
            m_array = Empty;

        else m_array = new T[capacity];
    }

    public List_t(T[] data)
    {
        m_array = data ?? throw new ArgumentNullException();
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(m_array);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item) => Insert(m_array.Length, item);

    public void Clear() => Array.Clear(m_array);

    public bool Contains(T item) => IndexOf(item) != -1;

    public void CopyTo(T[] array, int arrayIndex) =>
        Array.Copy(m_array, 0, array, arrayIndex, m_array.Length);

    public bool Remove(T item)
    {
        int index;

        if ((index = IndexOf(item)) == -1)
            return false;

        RemoveAt(index);
        return true;
    }

    public int Count => m_array.Length;
    
    public bool IsReadOnly => false;

    public int IndexOf(T item) => Array.IndexOf(m_array, item, 0, m_array.Length);

    public void Insert(int index, T item)
    {
        int newSize = m_array.Length + 1;
        
        ThrowableInRange(0, index, newSize);

        Resize((uint)newSize);
        
        Array.Copy(m_array, index, m_array, index + 1, newSize - index - 1);

        m_array[index] = item;
    }

    public void RemoveAt(int index)
    {
        ThrowableInRange(0, index, m_array.Length);

        int size = m_array.Length;
        
        Array.Copy(m_array, index + 1, m_array, index, size - 1 - index);
        
        Resize((uint)(size - 1));
    }

    public T this[int index]
    {
        get
        {
            ThrowableInRange(0, index, m_array.Length);
            return m_array[index];
        }

        set
        {
            ThrowableInRange(0, index, m_array.Length);

            m_array[index] = value;
        }
    }

    public class Enumerator : IEnumerator<T>
    {
        private readonly T[] array;

        private T? current;

        private int index;
        
        internal Enumerator(T[] array)
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

        public T Current => current!;
        
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
        Array.Resize(ref m_array, (int)size);
    }
    
    private static void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }
}