using IO.converters.delegates;
using IO.responses;

namespace IO.converters;

public class ConsoleSimpleDataConverter<TOut> :
    IConvertibleData<string?, TOut>,
    ICloneable
{ 
    public SimpleDataConverter<string?, TOut> Expression { get; set; }

    public ConsoleSimpleDataConverter(SimpleDataConverter<string?, TOut> expression)
    {
        Expression = expression;
    }

    public IResponsibleData<TOut> Convert(IResponsibleData<string?> responsibleData)
    {
        if (!responsibleData.IsOk())
            return ConsoleResponseDataFactory.MakeResponse<TOut>(
                error: responsibleData.Error(), code: (ConsoleResponseDataCode) responsibleData.Code());

        if (!Expression.Invoke(responsibleData.Data(), out var value))
            return ConsoleResponseDataFactory.MakeResponse<TOut>(
                error: $"невозможно привести '{responsibleData.Data()} к '{typeof(TOut).Name}'");

        return ConsoleResponseDataFactory.MakeResponse(value);
    }

    public object Clone()
    {
        return new ConsoleSimpleDataConverter<TOut>(Expression);
    }
}