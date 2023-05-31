using System.Globalization;
using System.Reflection;

namespace UserDataRequester.converters.parsers;

public static class BaseParserInvoker
{
    public static bool Invoke(MethodInfo method, object? data, out object output)
    {
        output = default!;
        
        object?[] args = { data, output };

        if (method.Invoke(null, args) is not bool isOk)
            return false;

        output = args[1]!;
        return isOk;
    }
    
    public static bool Invoke(
        MethodInfo method, 
        object? data, 
        out object output,
        NumberStyles styles,
        IFormatProvider? provider)
    {
        output = default!;
        
        object?[] args = { data, styles, provider, output };

        if (method.Invoke(null, args) is not bool isOk)
            return false;

        output = args[3]!;
        return isOk;
    }
}