using labs.abstracts;
using labs.IO;

namespace labs.builders;

public class ConsoleDataResponseBuilder<T> :
    DataResponseBuilder<T>
{
    public ConsoleDataResponseBuilder(ConsoleDataResponse<T>? obj = default) : 
        base(obj ?? new ConsoleDataResponse<T>())
    { }

    public override ConsoleDataResponseBuilder<T> Data(T value)
    {
        return (ConsoleDataResponseBuilder<T>) base.Data(value);
    }

    public override ConsoleDataResponseBuilder<T> Error(string? value)
    {
        return (ConsoleDataResponseBuilder<T>)base.Error(value);
    }

    public override ConsoleDataResponseBuilder<T> Code(int value)
    {
        return (ConsoleDataResponseBuilder<T>)base.Code(value);
    }
}