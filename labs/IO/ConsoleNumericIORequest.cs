using labs.interfaces;

namespace labs.IO;

public class ConsoleNumericIORequest<T> :
    IDataIORequest<T>
{
    public ConsoleIOTarget ConsoleTarget { get; set; }
    
    public Converter<string?, T> ConsoleIOConverter { get; set; } 

    public string Message { get; set; }
    
    public ConsoleNumericIORequest(
        string message,
        Converter<string?, T> consoleIoConverter,
        ConsoleIOTarget? consoleTarget = default)
    {
        Message = message;
        ConsoleTarget = consoleTarget ?? new ConsoleIOTarget();
        ConsoleIOConverter = consoleIoConverter;
    }
    
    public IDataIOResponse<T> Request(DataIoValidator<T> validator)
    {
        string? buffer;

        ConsoleTarget.Write(Message);

        if ((buffer = ConsoleTarget.Read()) == null)
            return new ConsoleNumericIOResponse<T>(default, "Ошибка: ввод прекращен");

        T? data = ConsoleIOConverter(buffer, out var error);

        if (error != null)
            return new ConsoleNumericIOResponse<T>(default, error);

        if (!validator.Invoke(data))
            error = $"Ошибка: значение {data} не удовлетворяет условиям";
        
        return new ConsoleNumericIOResponse<T>((error != null) ? default : data, error);
    }
}