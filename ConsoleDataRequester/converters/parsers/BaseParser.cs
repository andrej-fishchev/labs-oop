using System.Globalization;
using System.Reflection;

namespace UserDataRequester.converters.parsers;

public static class BaseParser
{
    public const string BaseParserSignature = "TryParse";
    
    public static MethodInfo? GetTypeParser<T>(
        string signature,
        Type[] paramTypes,
        ParameterModifier[]? paramModifiers = default,
        BindingFlags flags = BindingFlags.Public | BindingFlags.Static,
        Binder? binder = default,
        CallingConventions callingConventions = CallingConventions.Any) =>
        typeof(T).GetMethod(signature, flags, binder, callingConventions, paramTypes, paramModifiers);

    public static MethodInfo? TryParseSignature<T>() => GetTypeParser<T>(
        BaseParserSignature, 
        new[]{ typeof(string), typeof(T).MakeByRefType() }, 
        flags: BindingFlags.Public | BindingFlags.Static
    );
    
    public static MethodInfo? TryParseSignatureWithStylesAndFormat<T>() => GetTypeParser<T>(
        BaseParserSignature, 
        new[]{ typeof(string), typeof(NumberStyles), typeof(IFormatProvider), typeof(T).MakeByRefType() }, 
        flags: BindingFlags.Public | BindingFlags.Static
    );
    
    public static bool TryParseObject(MethodInfo? method, object? data, out object output)
    {
        output = default!;

        return method != null && BaseParserInvoker.Invoke(method, data, out output);
    }
    
    public static bool TryParseNumber(
        MethodInfo? method, 
        object? data, 
        out object output, 
        NumberStyles styles, 
        IFormatProvider? provider = default)
    {
        output = default!;

        return method != null && BaseParserInvoker.Invoke(method, data, out output, styles, provider);
    }
}