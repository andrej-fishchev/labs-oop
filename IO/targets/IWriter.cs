namespace IO;

public interface IWriter<in T>
{
    public void Write(T data);
}