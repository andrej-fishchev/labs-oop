using System.Reflection;
using UserDataRequester.converters.delegates;
using UserDataRequester.responses;
using UserDataRequester.responses.console;

namespace UserDataRequester.converters.console;

public class ConsoleSimpleDataConverter :
    IConvertibleData,
    ICloneable
{
    public MethodInfo? TypeParserSignature { get; set; }
    
    public SimpleObjectConverterDelegate Expression { get; set; }

    public ConsoleSimpleDataConverter(SimpleObjectConverterDelegate expression, MethodInfo? typeParserSignature)
    {
        Expression = expression;
        TypeParserSignature = typeParserSignature;
    }
    
    public IResponsibleData<object> Convert(IResponsibleData<object> data)
    {
        object? value = default;
        ResponseStatusCode? code = default;

        if (TypeParserSignature == null)
            code = ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.InvalidParserSignature);

        if (code == null && !data.IsOk())
            code = data.StatusCode();

        if (code == null && !Expression.Invoke(TypeParserSignature, data.Data(), out value))
            code = ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.UnableToConvertData);

        return ConsoleResponseDataFactory.MakeResponse(value, code);
    }
    
    public object Clone() => new ConsoleSimpleDataConverter(Expression, TypeParserSignature);
}