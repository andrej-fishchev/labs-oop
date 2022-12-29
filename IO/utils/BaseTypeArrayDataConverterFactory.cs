using System.Globalization;
using IO.converters;
using IO.validators;

namespace IO.utils;

public static class BaseTypeArrayDataConverterFactory
{
    public static SimpleConsoleArrayDataConverter<int> MakeIntArrayConverter(
        IValidatableData<int>? nestedValidator = default,
        string delimiter = ";")
    {
        return new SimpleConsoleArrayDataConverter<int>(
            BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
            nestedValidator,
            delimiter);
    }

    public static SimpleConsoleArrayDataConverter<double> MakeDoubleArrayConverter(
        IValidatableData<double>? nestedValidator = default,
        string delimiter = ";")

    {
        return new SimpleConsoleArrayDataConverter<double>(
            BaseTypeDataConverterFactory.MakeSimpleDoubleConverter(),
            nestedValidator,
            delimiter);
    }
    
    public static SimpleConsoleArrayDataConverter<float> MakeFloatArrayConverter(
        IValidatableData<float>? nestedValidator = default,
        string delimiter = ";")

    {
        return new SimpleConsoleArrayDataConverter<float>(
            BaseTypeDataConverterFactory.MakeSimpleFloatConverter(),
            nestedValidator,
            delimiter);
    }
    
    public static SimpleConsoleArrayDataConverter<string> MakeStringArrayConverter(
        IValidatableData<string>? nestedValidator = default,
        string delimiter = ";")

    {
        return new SimpleConsoleArrayDataConverter<string>(
            BaseTypeDataConverterFactory.MakeSimpleStringConverter(),
            nestedValidator,
            delimiter);
    }

    public static SimpleConsoleArrayDataConverter<double> MakeFormattedDoubleArrayConverter(
        IValidatableData<double>? nestedValidator = default,
        string delimiter = ";")
    {
        return new SimpleConsoleArrayDataConverter<double>(
            BaseTypeDataConverterFactory.MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo),
            nestedValidator,
            delimiter);
    }
}