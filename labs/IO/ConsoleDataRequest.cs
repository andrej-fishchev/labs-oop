using labs.interfaces;

namespace labs.IO;

public class ConsoleDataRequest<T> :
    IDataRequest<T>,
    ICloneable
{
    public string DisplayMessage { get; set; }
    
    public ConsoleTarget Target { get; set; }
    
    public ConsoleDataConverter<T> Converter { get; set; }

    public string RejectRequestMessage { get; set; }
    
    public ConsoleDataRequest(string message, ConsoleDataConverter<T> converter, 
        string rejectRequestMessage = "...")
    {
        DisplayMessage = message;
        Converter = converter;
        RejectRequestMessage = rejectRequestMessage;
        
        Target = new ConsoleTarget();
    }

    public IDataResponse<T> Request(IDataValidator<T>? validator = default, bool sendRejectMessage = true)
    {
        if (validator != null &&  validator is not ConsoleDataValidator<T>)
            throw new InvalidCastException("Ожидался объект типа ConsoleDataValidator");
        
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
                buffer = (outputResponse = (ConsoleDataResponse<T>) validator
                    .Validate(outputResponse)).Error;
            
            if(buffer != null)
                Target.Write($"Ошибка: {buffer} \n");
        } 
        
        return outputResponse;
    }

    public object Clone()
    {
        return new ConsoleDataRequest<T>(DisplayMessage, Converter, RejectRequestMessage);
    }
}