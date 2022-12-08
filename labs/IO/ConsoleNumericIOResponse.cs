using labs.interfaces;

namespace labs.IO;

public class ConsoleNumericIOResponse<T> :
    IDataIOResponse<T>
{
    public ConsoleNumericIOResponse(T? data, string? error = null)
    {
        Data = data;
        Error = error;
    }

    public T? Data { get; set; }
    
    public string? Error { get; set; }
}