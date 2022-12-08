namespace labs.interfaces;

public interface IDataIOResponse<T>
{
    public T? Data { get; set; }
    
    public string? Error { get; set; }
}