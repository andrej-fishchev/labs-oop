using labs.interfaces;

namespace labs.IO;

public class ConsoleDataResponse<T> :
    IDataIoResponse<T>
{
    public ConsoleDataResponse(T? data = default, string? error = default)
    {
        Data = data;
        Error = error;
    }

    public T? Data { get; set; }
    
    public string? Error { get; set; }
}