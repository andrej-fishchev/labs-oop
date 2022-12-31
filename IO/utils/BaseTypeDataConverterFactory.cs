using System.Globalization;
using IO.converters;

namespace IO.utils;

public static class BaseTypeDataConverterFactory
{
    public static ConsoleSimpleDataConverter<double> MakeSimpleDoubleConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<double>(double.TryParse);
    }

    public static ConsoleSimpleDataConverter<int> MakeSimpleIntConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<int>(int.TryParse);
    }

    public static ConsoleSimpleDataConverter<string> MakeSimpleStringConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter(
            ((string? data, out string output) => 
            {
                output = data ?? "";
                return data != null;
            })
        );
    }

    public static ConsoleSimpleDataConverter<float> MakeSimpleFloatConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<float>(float.TryParse);
    }

    public static ConsoleFormattedNumberDataConverter<double> MakeFormattedDoubleConverter(
        NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands,
        IFormatProvider? provider = default)
    {
        return ConsoleDataConverterFactory.MakeFormattedNumberDataConverter<double>(double.TryParse, styles, provider);
    }

    public static ConsoleDataConverterList<double> MakeChainedDoubleConverter()
    {
        return ConsoleDataConverterFactory.MakeConverterList(
            new List<IConvertibleData<string?, double>>
            {
                MakeSimpleDoubleConverter(),
                MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo)
            });
    }
}