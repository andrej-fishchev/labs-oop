namespace labs.shared.data.abstracts;

public interface IDeque<T>
{
    public T Back();
    public void Back(T value);
    
    public T Front();
    public void Front(T value);

    public void RemoveAt(int index);

    public T this[int index]
    {
        get;
        set;
    }
}