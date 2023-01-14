using System.Globalization;
using IO.converters;

namespace IO.utils;

public static class BaseTypeDataConverterFactory
{
    public static ConsoleSimpleDataConverter<double> MakeSimpleDoubleConverter() => 
        ConsoleDataConverterFactory.MakeSimpleConverter<double>(double.TryParse);

    public static ConsoleSimpleDataConverter<int> MakeSimpleIntConverter() => 
        ConsoleDataConverterFactory.MakeSimpleConverter<int>(int.TryParse);

    public static ConsoleSimpleDataConverter<string> MakeSimpleStringConverter() => 
        ConsoleDataConverterFactory.MakeSimpleConverter((string? data, out string output) => 
            {
                output = data ?? "";
                return data != null;
            }
        );

    public static ConsoleSimpleDataConverter<float> MakeSimpleFloatConverter() => 
        ConsoleDataConverterFactory.MakeSimpleConverter<float>(float.TryParse);

    public static ConsoleFormattedNumberDataConverter<double> 
        MakeFormattedDoubleConverter(NumberStyles styles = NumberStyles.Float | NumberStyles.AllowThousands, 
            IFormatProvider? provider = default) => ConsoleDataConverterFactory
        .MakeFormattedNumberDataConverter<double>(double.TryParse, styles, provider);

    public static ConsoleDataConverterList<double> MakeDoubleConverterList() => 
        ConsoleDataConverterFactory.MakeConverterList(new List<IConvertibleData<string?, double>> 
        {
                MakeSimpleDoubleConverter(),
                MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo)
        });
}