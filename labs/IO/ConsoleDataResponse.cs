using labs.interfaces;

namespace labs.IO;

public class ConsoleDataResponse<T> :
    IDataIoResponse<T>
{
    public ConsoleDataResponse(T? data = default, string? error = default, int code = default)
    {
        Data = data;
        Error = error;
        Code = code;
    }

    public T? Data { get; set; }
    
    public string? Error { get; set; }
    
    public int Code { get; set; }
}