using UserDataRequester.converters.console;
using UserDataRequester.validators;

namespace UserDataRequester.converters.console.utils;

public static class BaseArrayDataConverterFactory
{
    public static ConsoleArrayDataConverter
        MakeIntArrayConverter(IValidatableData? nestedValidator = default, string delimiter = ";")
    {
        return new ConsoleArrayDataConverter(
            delimiter,
            BaseDataConverterFactory.MakeSimpleIntConverter(),
            nestedValidator
        );
    }
    
    public static ConsoleArrayDataConverter
        MakeStringArrayConverter(IValidatableData? nestedValidator = default, string delimiter = ";")
    {
        return new ConsoleArrayDataConverter(
            delimiter,
            default,
            nestedValidator
        );
    }
//
//     public static ConsoleArrayDataConverter<double>
//         MakeDoubleArrayConverter(IValidatableData<double>? nestedValidator = default, string delimiter = ";")
//     {
//         return new ConsoleArrayDataConverter<double>(BaseTypeDataConverterFactory.MakeSimpleDoubleConverter(),
//             nestedValidator, delimiter);
//     }
//
//     public static ConsoleArrayDataConverter<float>
//         MakeFloatArrayConverter(IValidatableData<float>? nestedValidator = default, string delimiter = ";")
//     {
//         return new ConsoleArrayDataConverter<float>(BaseTypeDataConverterFactory.MakeSimpleFloatConverter(),
//             nestedValidator, delimiter);
//     }
//
//     public static ConsoleArrayDataConverter<string>
//         MakeStringArrayConverter(IValidatableData<string>? nestedValidator = default, string delimiter = ";")
//     {
//         return new ConsoleArrayDataConverter<string>(BaseTypeDataConverterFactory.MakeSimpleStringConverter(),
//             nestedValidator, delimiter);
//     }
//
//     public static ConsoleArrayDataConverter<double>
//         MakeFormattedDoubleArrayConverter(IValidatableData<double>? nestedValidator = default, string delimiter = ";")
//     {
//         return new ConsoleArrayDataConverter<double>(
//             BaseTypeDataConverterFactory.MakeFormattedDoubleConverter(provider: NumberFormatInfo.InvariantInfo),
//             nestedValidator,
//             delimiter);
//     }
//
//     public static ConsoleArrayDataConverter<double>
//         MakeDoubleArrayConverterList(IValidatableData<double>? nestedValidator = default, string delimiter = ";")
//     {
//         return ConsoleDataConverterFactory.MakeArrayConverter(
//             ConsoleDataConverterFactory.MakeConverterList(BaseTypeDataConverterFactory.MakeDoubleConverterList()),
//             nestedValidator,
//             delimiter);
//     }
}