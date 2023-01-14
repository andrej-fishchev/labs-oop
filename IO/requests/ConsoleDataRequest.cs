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
    
    public string RejectKey { get; set; }
    
    public ConsoleDataRequest(string message, string rejectInputKey = "...")
    {
        DisplayMessage = message;
        RejectKey = rejectInputKey;
    }

    public IResponsibleData<TOut> Request(
        IConvertibleData<string?, TOut> converter, 
        IValidatableData<TOut>? validator = default, 
        bool sendRejectMessage = true)
    {
        if(sendRejectMessage)
            Target.Output
                .WriteLine("Ввод может быть прекращен в любое удобное время. \n" 
                           + $"Введите '{RejectKey}' для незамедлительного прекращения \n");

        ConsoleResponseData<TOut> output;

        do
        {
            Target.Output.WriteLine(DisplayMessage);

            // in: error is null already
            string? buffer;
            if ((buffer = Target.Input.ReadLine()) == null)
                return converter.Convert(ConsoleResponseDataFactory.MakeResponse(
                    buffer, code: ConsoleResponseDataCode.ConsoleTerminated));

            // in: error is null already
            if (buffer.Trim().Equals(RejectKey))
                return converter.Convert(ConsoleResponseDataFactory.MakeResponse<string?>(
                    buffer, code: ConsoleResponseDataCode.ConsoleInputRejected));

            // in: error is null already
            output = converter.Convert(ConsoleResponseDataFactory.MakeResponse<string?>(
                    buffer, code: ConsoleResponseDataCode.ConsoleOk))
                .As<ConsoleResponseData<TOut>>();

            if (output.IsOk() && validator != null)
                output = validator.Validate(output).As<ConsoleResponseData<TOut>>();

            if (output.HasError())
                Target.Output.WriteLine($"Ошибка: {output.Error()} \n");

        } while (!output.IsOk());
        
        return output;
    }
    
    public object Clone()
    {
        return new ConsoleDataRequest<TOut>(DisplayMessage, RejectKey);
    }
}