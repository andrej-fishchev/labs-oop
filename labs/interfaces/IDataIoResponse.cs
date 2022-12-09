namespace labs.interfaces;

public interface IDataIoResponse<T>
{
    public T? Data { get; set; }
    
    public string? Error { get; set; }
}