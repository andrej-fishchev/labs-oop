using IO.converters;
using IO.responses;
using IO.targets;
using IO.validators;

namespace IO.requests;

public class ConsoleArrayDataRequest<TResponse> :
    IRequestableData<string?, TResponse[]>,
    ICloneable
{
    public readonly ConsoleTarget Target = new();
    
    public string DisplayMessage { get; set; }
    
    public string RejectKey { get; set; }

    public ConsoleArrayDataRequest(string displayMessage, string rejectKey = "...")
    {
        DisplayMessage = displayMessage;
        RejectKey = rejectKey;
    }

    // [1<,> 2<,> ...] 
    public IResponsibleData<TResponse[]> Request(
        IConvertibleData<string?, TResponse[]> converter, 
        IValidatableData<TResponse[]>? validator = default, 
        bool sendRejectMessage = true)
    {
        string? buffer = "";

        ConsoleResponseData<TResponse[]> output = 
            new ConsoleResponseData<TResponse[]>();
        
        if(sendRejectMessage)
            Target.Write("Ввод может быть прекращен в любое удобное время. \n" + 
                         $"Введите '{RejectKey}' для незамедлительного прекращения\n\n");

        while(buffer != null)
        {
            Target.Write(DisplayMessage);

            if ((buffer = Target.Read()) == null)
                return converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int)ConsoleResponseDataCode.ConsoleTerminated));

            if (buffer.Trim().Equals(RejectKey))
                return converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int) ConsoleResponseDataCode.ConsoleInputRejected));

            output = (ConsoleResponseData<TResponse[]>) 
                converter.Convert(new ConsoleResponseData<string?>(
                    buffer, code: (int)ConsoleResponseDataCode.ConsoleOk));

            if ((buffer = output.Error) == null && validator != null)
                buffer = (output = (ConsoleResponseData<TResponse[]>) validator
                    .Validate(output)).Error;
            
            if(buffer != null)
                Target.Write($"Ошибка: {buffer} \n");
        } 
        
        return output;
    }

    public object Clone()
    {
        return new ConsoleArrayDataRequest<TResponse>(DisplayMessage, RejectKey);
    }
}