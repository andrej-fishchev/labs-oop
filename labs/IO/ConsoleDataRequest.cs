using labs.interfaces;

namespace labs.IO;

public class ConsoleDataRequest<T> :
    IDataIoRequest<T>
{
    public ConsoleIoTarget ConsoleTarget { get; set; }
    
    public DataConverter<string?, T> ConsoleDataConverter { get; set; }

    public string Message { get; set; }
    
    public ConsoleDataRequest(string message, 
        DataConverter<string?, T> converter, 
        ConsoleIoTarget? consoleTarget = default)
    {
        Message = message;
        ConsoleDataConverter = converter;
        ConsoleTarget = consoleTarget ?? new ConsoleIoTarget();
    }
    
    public IDataIoResponse<T> Request(IDataIoResponseConverter<T> converter)
    {
        ConsoleTarget.Write(Message);

        var response = new ConsoleDataResponse<string?>(ConsoleTarget.Read());

        if (response.Data == null)
            response.Error = "ввод прекращен";
        
        return converter.Convert(response, ConsoleDataConverter);
    }
}