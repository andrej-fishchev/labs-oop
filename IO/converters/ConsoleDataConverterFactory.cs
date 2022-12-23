using System.Globalization;

namespace IO.converters;

public static class ConsoleDataConverterFactory
{
    public static SimpleConsoleDataConverter<TOut> 
        MakeSimpleConverter<TOut>(SimpleDataConverter<string?, TOut> expression)
    {
        return new SimpleConsoleDataConverter<TOut>(expression);
    }

    public static FormattedConsoleNumberDataConverter<TOut> 
        MakeFormattedNumberDataConverter<TOut>(FormattedNumberDataConverter<string?, TOut> expression,
        NumberStyles s, IFormatProvider? f = default)
    {
        return new FormattedConsoleNumberDataConverter<TOut>(expression, s, f);
    }
}