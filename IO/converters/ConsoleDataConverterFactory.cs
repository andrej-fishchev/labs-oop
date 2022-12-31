using System.Globalization;
using IO.converters.delegates;
using IO.validators;

namespace IO.converters;

public static class ConsoleDataConverterFactory
{
    public static ConsoleSimpleDataConverter<TOut> 
        MakeSimpleConverter<TOut>(SimpleDataConverter<string?, TOut> expression)
    {
        return new ConsoleSimpleDataConverter<TOut>(expression);
    }

    public static ConsoleFormattedNumberDataConverter<TOut> 
        MakeFormattedNumberDataConverter<TOut>(FormattedNumberDataConverter<string?, TOut> expression,
        NumberStyles s, IFormatProvider? f = default)
    {
        return new ConsoleFormattedNumberDataConverter<TOut>(expression, s, f);
    }

    public static ConsoleDataConverterList<TOut>
        MakeConverterList<TOut>(IList<IConvertibleData<string?, TOut>> list)
    {
        return new ConsoleDataConverterList<TOut>(list);
    }

    public static ConsoleArrayDataConverter<TOut>
        MakeArrayConverter<TOut>(
            IConvertibleData<string?, TOut> nestedConverter,
            IValidatableData<TOut>? nestedValidator = default,
            string delimiter = ";")
    {
        return new ConsoleArrayDataConverter<TOut>(nestedConverter, nestedValidator, delimiter);
    }
}