namespace IO.targets;

public interface IReader<out T>
{
    public T Read();
}