using System.Globalization;
using System.Reflection;
using UserDataRequester.converters.parsers;

namespace UserDataRequester.converters.console.utils;

public static class BaseDataConverterFactory
{
    public static ConsoleSimpleDataConverter MakeObjectConverter(MethodInfo? signature) =>
        ConsoleDataConverterFactory.MakeSimpleObjectConverter(BaseParser.TryParseObject, signature);

    public static ConsoleSimpleDataConverter MakeSimpleIntConverter() =>
        MakeObjectConverter(BaseParser.GetTypeParser<int>(BaseParser.BaseParserSignature));
    
    public static ConsoleSimpleNumberConverter MakeNumberConverter(
        MethodInfo? signature,
        NumberStyles styles = NumberStyles.Any,
        IFormatProvider? provider = default
    ) => ConsoleDataConverterFactory.MakeSimpleNumberConverter(BaseParser.TryParseNumber, signature, styles, provider);

    public static ConsoleSimpleNumberConverter MakeDoubleConverter(
        NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands,
        IFormatProvider? provider = default
    ) => MakeNumberConverter(BaseParser.TryParseSignatureWithStylesAndFormat<double>(), styles, provider);

    public static ConsoleDataConverterList MakeDoubleConverterList() =>
        ConsoleDataConverterFactory.MakeConverterList(new List<IConvertibleData>
        {
            MakeDoubleConverter(),
            MakeDoubleConverter(provider: NumberFormatInfo.InvariantInfo)
        });
}