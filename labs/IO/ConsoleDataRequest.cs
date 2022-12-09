using labs.interfaces;

namespace labs.IO;

public class ConsoleDataRequest<T> :
    IDataIoRequest<T>
{
    public ConsoleIoTarget ConsoleTarget { get; set; }
    
    public DataConverter<string?, T> ConsoleDataConverter { get; set; }

    public string Message { get; set; }
    
    public string RejectInputMessage { get; set; }
    
    public ConsoleDataRequest(string message, 
        DataConverter<string?, T> converter, 
        ConsoleIoTarget? consoleTarget = default)
    {
        Message = message;
        RejectInputMessage = "--stop";
        ConsoleDataConverter = converter;
        ConsoleTarget = consoleTarget ?? new ConsoleIoTarget();
    }
    
    public IDataIoResponse<T> Request(IDataIoResponseConverter<T> converter)
    {
        ConsoleTarget.Write(Message);

        var response = new ConsoleDataResponse<string?>(ConsoleTarget.Read());

        if (response.Data == null)
        {
            response.Error = "ввод прекращен";
            response.Code = (int)ConsoleDataResponseCode.CONSOLE_TERMINATED;
        }

        if (RejectInputMessage.Length != 0
            && response.Code == (int)ConsoleDataResponseCode.CONSOLE_OK
            && response.Data!.Trim().Equals(RejectInputMessage))
            response.Code = (int)ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED;
        
        return converter.Convert(response, ConsoleDataConverter);
    }
}