using System.Globalization;
using IO.converters;

namespace IO.utils;

public static class BaseTypeDataConverterFactory
{
    public static SimpleConsoleDataConverter<double> MakeSimpleDoubleConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<double>(double.TryParse);
    }

    public static SimpleConsoleDataConverter<int> MakeSimpleIntConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<int>(int.TryParse);
    }

    public static SimpleConsoleDataConverter<string> MakeSimpleStringConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter(
            ((string? data, out string output) => 
            {
                output = data ?? "";
                return data != null;
            })
        );
    }

    public static SimpleConsoleDataConverter<float> MakeSimpleFloatConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<float>(float.TryParse);
    }

    public static FormattedConsoleNumberDataConverter<double> MakeFormattedDoubleConverter(
        NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands,
        IFormatProvider? provider = default)
    {
        return ConsoleDataConverterFactory.MakeFormattedNumberDataConverter<double>(double.TryParse, styles, provider);
    }

    public static ChainedConsoleDataConverter<double> MakeChainedDoubleConverter()
    {
        return ConsoleDataConverterFactory.MakeChainedConverter(
            new List<IConvertibleData<string?, double>>
            {
                MakeSimpleDoubleConverter(),
                MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo)
            });
    }
}