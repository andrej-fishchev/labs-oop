namespace IO.responses;

public class ConsoleResponseDataBuilder<T> :
    ResponseDataBuilder<T>
{
    public ConsoleResponseDataBuilder(ConsoleResponseData<T>? obj = default) : 
        base(obj ?? new ConsoleResponseData<T>())
    { }

    public override ConsoleResponseDataBuilder<T> Data(T value)
    {
        return (ConsoleResponseDataBuilder<T>) base.Data(value);
    }

    public override ConsoleResponseDataBuilder<T> Error(string? value)
    {
        return (ConsoleResponseDataBuilder<T>)base.Error(value);
    }

    public override ConsoleResponseDataBuilder<T> Code(int value)
    {
        return (ConsoleResponseDataBuilder<T>)base.Code(value);
    }
}