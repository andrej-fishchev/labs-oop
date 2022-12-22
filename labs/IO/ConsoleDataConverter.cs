using labs.delegates;
using labs.interfaces;

namespace labs.IO;

public class ConsoleDataConverter<TOut> :
    IDataConverter<string?, TOut>,
    ICloneable
{
    public DataConverter<string?, TOut> Expression { get; init; }

    public ConsoleDataConverter(DataConverter<string?, TOut> expression)
    {
        Expression = expression;
    }

    public IDataResponse<TOut> Convert(IDataResponse<string?> data)
    {
        ConsoleDataResponse<TOut> output = new ConsoleDataResponse<TOut>(
            code: data.Code, error: data.Error);

        if (data.Data == null)
            return output;

        output.Data = Expression.Invoke(data.Data, out var error);
        output.Error = error;

        return output;
    }

    public object Clone()
    {
        return new ConsoleDataConverter<TOut>(Expression);
    }
}