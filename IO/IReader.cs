namespace IO;

public interface IReader<out T>
{
    public T Read();
}