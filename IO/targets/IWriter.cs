namespace IO.targets;

public interface IWriter<in T>
{
    public void Write(T data);
}