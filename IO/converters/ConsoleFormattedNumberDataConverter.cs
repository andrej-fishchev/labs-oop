using System.Globalization;
using IO.converters.delegates;
using IO.responses;

namespace IO.converters;

public class ConsoleFormattedNumberDataConverter<TOut> :
    IConvertibleData<string?, TOut>,
    ICloneable
{
    public FormattedNumberDataConverter<string?, TOut> Expression
    {
        get; 
        set;
    }

    public NumberStyles Styles { get; set; }
    
    public IFormatProvider? Provider { get; set; }

    public ConsoleFormattedNumberDataConverter(FormattedNumberDataConverter<string?, TOut> expression,
        NumberStyles styles, IFormatProvider? provider = default)
    {
        Expression = expression;
        Styles = styles;
        Provider = provider;
    }

    public IResponsibleData<TOut> Convert(IResponsibleData<string?> responsibleData)
    {
        if (!responsibleData.IsOk())
            return ConsoleResponseDataFactory.MakeResponse<TOut>(
                error: responsibleData.Error(), code: (ConsoleResponseDataCode) responsibleData.Code());
        
        if (!Expression.Invoke(responsibleData.Data(), Styles, Provider, out var value))
            return ConsoleResponseDataFactory.MakeResponse<TOut>(
                error: $"невозможно привести '{responsibleData.Data()} к '{typeof(TOut).Name}'");

        return ConsoleResponseDataFactory.MakeResponse(value);
    }

    public object Clone()
    {
        return new ConsoleFormattedNumberDataConverter<TOut>(Expression, Styles, Provider);
    }
}