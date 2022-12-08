namespace labs.interfaces;

public interface IReader<out T>
{
    public T Read();
}