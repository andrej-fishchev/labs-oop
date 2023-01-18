using System.Collections;

namespace labs.lab9.src;

public class ListT<T> :
    IList<T>,
    ICloneable
{
    private T[] mArray;
    
    private static readonly T[] Empty = Array.Empty<T>();

    public ListT(int capacity = 0)
    {
        ThrowableInRange(0, capacity, Int32.MaxValue);

        if (capacity == 0)
            mArray = Empty;

        else mArray = new T[capacity];
    }

    public ListT(T[] data)
    {
        mArray = data ?? throw new ArgumentNullException();
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(mArray);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item) => Insert(mArray.Length, item);

    public void Clear() => Array.Clear(mArray);

    public bool Contains(T item) => IndexOf(item) != -1;

    public void CopyTo(T[] array, int arrayIndex) =>
        Array.Copy(mArray, 0, array, arrayIndex, mArray.Length);

    public bool Remove(T item)
    {
        int index;

        if ((index = IndexOf(item)) == -1)
            return false;

        RemoveAt(index);
        return true;
    }

    public int Count => mArray.Length;
    
    public bool IsReadOnly => false;

    public int IndexOf(T item) => Array.IndexOf(mArray, item, 0, mArray.Length);

    public void Insert(int index, T item)
    {
        int newSize = mArray.Length + 1;
        
        ThrowableInRange(0, index, newSize);

        Resize((uint)newSize);
        
        Array.Copy(mArray, index, mArray, index + 1, newSize - index - 1);

        mArray[index] = item;
    }

    public void RemoveAt(int index)
    {
        ThrowableInRange(0, index, mArray.Length);

        int size = mArray.Length;
        
        Array.Copy(mArray, index + 1, mArray, index, size - 1 - index);
        
        Resize((uint)(size - 1));
    }

    public T this[int index]
    {
        get
        {
            ThrowableInRange(0, index, mArray.Length);
            return mArray[index];
        }

        set
        {
            ThrowableInRange(0, index, mArray.Length);

            mArray[index] = value;
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
        
#pragma warning disable CS8603
        object IEnumerator.Current
        {
            get
            {
                ThrowableInRange(0, index, array.Length);
                
                return Current;
            }
        }
#pragma warning restore CS8603
        
        public void Dispose()
        { }
    }

    private void Resize(uint size)
    {
        Array.Resize(ref mArray, (int)size);
    }
    
    private static void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }

    public object Clone()
    {
        return new List<T>(mArray);
    }
}