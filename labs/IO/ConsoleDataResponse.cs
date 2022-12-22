using labs.interfaces;

namespace labs.IO;

public class ConsoleDataResponse<T> :
    IDataResponse<T>,
    ICloneable
{
    public ConsoleDataResponse(T data = default(T), string? error = default, int code = default)
    {
        Data = data;
        Error = error;
        Code = code;
    }

    public T Data { get; set; }
    
    public string? Error { get; set; }
    
    public int Code { get; set; }
    
    public object Clone()
    {
        return new ConsoleDataResponse<T>(Data, Error, Code);
    }
}