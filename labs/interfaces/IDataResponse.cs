namespace labs.interfaces;

public interface IDataResponse<T>
{
    public T Data { get; set; }
    
    public string? Error { get; set; }
    
    public int Code { get; set; }
}