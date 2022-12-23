using IO.converters;
using IO.responses;
using IO.targets;
using IO.validators;

namespace IO.requests;

public class ConsoleDataRequest<TOut> :
    IRequestableData<string?, TOut>,
    ICloneable
{
    public readonly ConsoleTarget Target = new();
    
    public string DisplayMessage { get; set; }
    
    public string RejectRequestMessage { get; set; }
    
    public ConsoleDataRequest(string message, string rejectRequestMessage = "...")
    {
        DisplayMessage = message;
        RejectRequestMessage = rejectRequestMessage;
    }

    public IResponsibleData<TOut> Request(
        IConvertibleData<string?, TOut> converter, 
        IValidatableData<TOut>? validator = default, 
        bool sendRejectMessage = true)
    {
        string? buffer = "";

        ConsoleResponseData<TOut> output = 
            new ConsoleResponseData<TOut>();
        
        if(sendRejectMessage)
            Target.Write("Ввод может быть прекращен в любое удобное время. \n" + 
                         $"Введите '{RejectRequestMessage}' для незамедлительного прекращения\n\n");

        while(buffer != null)
        {
            Target.Write(DisplayMessage);

            if ((buffer = Target.Read()) == null)
                return converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int)ConsoleResponseDataCode.ConsoleTerminated));

            if (buffer.Trim().Equals(RejectRequestMessage))
                return converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int) ConsoleResponseDataCode.ConsoleInputRejected));

            output = (ConsoleResponseData<TOut>) 
                converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int)ConsoleResponseDataCode.ConsoleOk));

            if ((buffer = output.Error) == null && validator != null)
                buffer = (output = (ConsoleResponseData<TOut>) validator
                    .Validate(output)).Error;
            
            if(buffer != null)
                Target.Write($"Ошибка: {buffer} \n");
        } 
        
        return output;
    }
    
    public object Clone()
    {
        return new ConsoleDataRequest<TOut>(DisplayMessage, RejectRequestMessage);
    }
}