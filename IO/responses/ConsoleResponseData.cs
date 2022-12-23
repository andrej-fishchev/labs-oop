namespace IO.responses;

public class ConsoleResponseData<T> :
    IResponsibleData<T>
{
    public T Data { get; set; }
    
    public string? Error { get; set; }
    
    public int Code { get; set; }
    
    public ConsoleResponseData(T data = default(T), string? error = default, int code = default)
    {
        Data = data;
        Error = error;
        Code = code;
    }

    public object Clone()
    {
        return new ConsoleResponseData<T>(Data, Error, Code);
    }
}