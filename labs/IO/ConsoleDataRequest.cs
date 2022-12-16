using labs.interfaces;

namespace labs.IO;

public class ConsoleDataRequest<T> :
    IDataIoRequest<T>
{
    public string DisplayMessage { get; set; }
    
    public ConsoleIoTarget Target { get; set; }
    
    public IDataIoResponseConverter<string?, T> Converter { get; set; }

    public string RejectRequestMessage { get; set; }
    
    public ConsoleDataRequest(string message, IDataIoResponseConverter<string?, T> converter, string rejectRequestMessage = "...")
    {
        DisplayMessage = message;
        Converter = converter;
        RejectRequestMessage = rejectRequestMessage;
        Target = new ConsoleIoTarget();
    }
    
    public IDataIoResponse<T> Request(IDataIoValidator<T>? validator = default, bool sendRejectMessage = true)
    {
        string? buffer = "";

        ConsoleDataResponse<T> outputResponse = 
            new ConsoleDataResponse<T>();
        
        if(sendRejectMessage)
            Target.Write("Ввод может быть прекращен в любое удобное время. \n" + 
                     $"Введите '{RejectRequestMessage}' для незамедлительного прекращения\n\n");

        while(buffer != null)
        {
            Target.Write(DisplayMessage);

            if ((buffer = Target.Read()) == null)
                return Converter.Convert(new ConsoleDataResponse<string?>(
                    buffer, code: (int)ConsoleDataResponseCode.ConsoleTerminated));

            if (buffer.Trim().Equals(RejectRequestMessage))
                return Converter.Convert(new ConsoleDataResponse<string?>(
                    default, code: (int)ConsoleDataResponseCode.ConsoleInputRejected));

            outputResponse = (ConsoleDataResponse<T>)Converter
                .Convert(new ConsoleDataResponse<string?>(buffer, code: (int)ConsoleDataResponseCode.ConsoleOk));

            if ((buffer = outputResponse.Error) == null && validator != null)
                buffer = (outputResponse = (ConsoleDataResponse<T>) validator.Validate(
                    outputResponse, $"значение '{outputResponse.Data}' не удовлетворяет условиям")).Error;
            
            if(buffer != null)
                Target.Write($"Ошибка: {buffer} \n");
        } 
        
        return outputResponse;
    }
}