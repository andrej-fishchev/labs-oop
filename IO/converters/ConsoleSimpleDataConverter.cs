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
        ConsoleResponseData<TOut> output = new ConsoleResponseData<TOut>(
            code: responsibleData.Code, error: responsibleData.Error);

        if (responsibleData.Code != (int)ConsoleResponseDataCode.ConsoleOk)
            return output;

        TOut value;
        if (!Expression.Invoke(responsibleData.Data, out value))
        {
            var declaringType = typeof(TOut).DeclaringType;

            if (declaringType != null)
                output.Error = $"не удалось привести '{responsibleData.Data}' к типу {declaringType.Name}";

            else output.Error = $"не удалось выполнить преобразование для '{responsibleData.Data}'";

            output.Code = (int)ConsoleResponseDataCode.ConsoleInvalidData;
        }

        output.Data = value;
        return output;
    }

    public object Clone()
    {
        return new ConsoleSimpleDataConverter<TOut>(Expression);
    }
}