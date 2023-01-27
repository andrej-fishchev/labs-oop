using System.Globalization;
using IO.converters;
using IO.validators;

namespace IO.utils;

public static class BaseTypeArrayDataConverterFactory
{
    public static ConsoleArrayDataConverter<int> 
        MakeIntArrayConverter(IValidatableData<int>? nestedValidator = default, string delimiter = ";") => 
        new(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), nestedValidator, delimiter);

    public static ConsoleArrayDataConverter<double> 
        MakeDoubleArrayConverter(IValidatableData<double>? nestedValidator = default, string delimiter = ";") => 
        new(BaseTypeDataConverterFactory.MakeSimpleDoubleConverter(), nestedValidator, delimiter);

    public static ConsoleArrayDataConverter<float> 
        MakeFloatArrayConverter(IValidatableData<float>? nestedValidator = default, string delimiter = ";") => 
        new(BaseTypeDataConverterFactory.MakeSimpleFloatConverter(), nestedValidator, delimiter);

    public static ConsoleArrayDataConverter<string> 
        MakeStringArrayConverter(IValidatableData<string>? nestedValidator = default, string delimiter = ";") => 
        new(BaseTypeDataConverterFactory.MakeSimpleStringConverter(), nestedValidator, delimiter);

    public static ConsoleArrayDataConverter<double> 
        MakeFormattedDoubleArrayConverter(IValidatableData<double>? nestedValidator = default, string delimiter = ";")
            => new(BaseTypeDataConverterFactory.MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo),
                nestedValidator,
                delimiter);

    public static ConsoleArrayDataConverter<double> 
        MakeDoubleArrayConverterList(IValidatableData<double>? nestedValidator = default, string delimiter = ";") => 
        ConsoleDataConverterFactory.MakeArrayConverter(
            ConsoleDataConverterFactory.MakeConverterList(BaseTypeDataConverterFactory.MakeDoubleConverterList()), 
            nestedValidator, 
            delimiter);
}