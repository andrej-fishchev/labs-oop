namespace labs.shared.data.abstracts;

public sealed class CustomHashTable<T>
{
    private Deque<T>[] mContainer;
    private int mSize;

    public const int DefaultContainerSize = 10;

    public CustomHashTable(int size = DefaultContainerSize)
    {
        ThrowableInRange(0, size, int.MaxValue);

        mSize = size;
        mContainer = new Deque<T>[mSize];
    }

    public bool Add(T value)
    {
        try
        {
            if (mContainer.FirstOrDefault(x => x.Contains(value)) != null)
                return false;
        }
        catch
        {
            // ignored
        }

        var index = GetIndex(value);

        mContainer[index] ??= new Deque<T>();
        
        mContainer[index].Back(value);
        
        return true;
    }

    public void Remove(T value)
    {
        try
        {
            mContainer.FirstOrDefault(x => x.Contains(value))?.Remove(value);
        }
        catch
        {
            // ignored
        }
    }

    public void Clear() => Array.Clear(mContainer);

    private int GetIndex(T value) => Math.Abs(value!.GetHashCode()) % mSize;

    private void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }

    public int Size() => mSize;
    public Deque<T>[] InnerContainer() => mContainer;
}