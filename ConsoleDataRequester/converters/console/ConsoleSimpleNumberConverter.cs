using System.Globalization;
using System.Reflection;
using UserDataRequester.converters.delegates;
using UserDataRequester.responses;
using UserDataRequester.responses.console;

namespace UserDataRequester.converters.console;

public class ConsoleSimpleNumberConverter :
    IConvertibleData,
    ICloneable
{
    public ConsoleSimpleNumberConverter(
        SimpleNumberConverterDelegate expression, 
        MethodInfo? typeParserSignature, 
        NumberStyles styles = NumberStyles.Any, 
        IFormatProvider? provider = default)
    {
        Expression = expression;
        TypeParserSignature = typeParserSignature;
        Styles = styles;
        Provider = provider;
    }

    public SimpleNumberConverterDelegate Expression { get; set; }

    public MethodInfo? TypeParserSignature { get; set; }

    public NumberStyles Styles { get; set; }

    public IFormatProvider? Provider { get; set; }

    public object Clone() => new ConsoleSimpleNumberConverter(Expression, TypeParserSignature, Styles, Provider);

    public IResponsibleData<object> Convert(IResponsibleData<object> responsibleData)
    {
        object? value = default;
        ResponseStatusCode? code = default;

        if (TypeParserSignature == null)
            code = ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.InvalidParserSignature);

        if (code == null && !responsibleData.IsOk())
            code = responsibleData.StatusCode();

        if (code == null && !Expression.Invoke(
                TypeParserSignature, 
                responsibleData.Data(), 
                out value, 
                Styles, 
                Provider
        )) code = ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.UnableToConvertData);

        return ConsoleResponseDataFactory.MakeResponse(value, code);
    }
}