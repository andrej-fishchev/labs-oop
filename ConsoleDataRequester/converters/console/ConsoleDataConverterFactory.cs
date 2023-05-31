using System.Globalization;
using System.Reflection;
using UserDataRequester.converters.delegates;

namespace UserDataRequester.converters.console;

public static class ConsoleDataConverterFactory
{
    public static ConsoleSimpleDataConverter MakeSimpleObjectConverter(
        SimpleObjectConverterDelegate expression, 
        MethodInfo? typeParserSignature
    ) => new(expression, typeParserSignature);

    public static ConsoleSimpleNumberConverter MakeSimpleNumberConverter(
        SimpleNumberConverterDelegate expression, 
        MethodInfo? typeParserSignature, 
        NumberStyles s = NumberStyles.Any, 
        IFormatProvider? f = default
    ) => new(expression, typeParserSignature, s, f);

    public static ConsoleDataConverterList MakeConverterList(IList<IConvertibleData> list) => 
        new(list);
}