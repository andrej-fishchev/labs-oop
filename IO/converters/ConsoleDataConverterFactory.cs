using System.Globalization;
using IO.validators;

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

    public static ChainedConsoleDataConverter<TOut>
        MakeChainedConverter<TOut>(IList<IConvertibleData<string?, TOut>> chain)
    {
        return new ChainedConsoleDataConverter<TOut>(chain);
    }

    public static SimpleConsoleArrayDataConverter<TOut>
        MakeSimpleArrayConverter<TOut>(
            IConvertibleData<string?, TOut> nestedConverter,
            IValidatableData<TOut>? nestedValidator = default,
            string delimiter = ";")
    {
        return new SimpleConsoleArrayDataConverter<TOut>(nestedConverter, nestedValidator, delimiter);
    }
}