using System.Globalization;
using IO.responses;

namespace IO.converters;

public class FormattedConsoleNumberDataConverter<TOut> :
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

    public FormattedConsoleNumberDataConverter(FormattedNumberDataConverter<string?, TOut> expression,
        NumberStyles styles, IFormatProvider? provider = default)
    {
        Expression = expression;
        Styles = styles;
        Provider = provider;
    }

    public IResponsibleData<TOut> Convert(IResponsibleData<string?> responsibleData)
    {
        ConsoleResponseData<TOut> response = new ConsoleResponseData<TOut>(
            code: responsibleData.Code, error: responsibleData.Error);
        
        TOut value;
        if (!Expression.Invoke(responsibleData.Data, Styles, Provider, out value))
        {
            var declaringType = typeof(TOut).DeclaringType;

            if (declaringType != null)
                response.Error = $"не удалось привести '{responsibleData.Data}' к типу {declaringType.Name}";

            else response.Error = $"не удалось выполнить преобразование для '{responsibleData.Data}'";
        }

        response.Data = value;
        return response;
    }

    public object Clone()
    {
        return new FormattedConsoleNumberDataConverter<TOut>(Expression, Styles, Provider);
    }
}