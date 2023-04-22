// using System.Globalization;
// using ConsoleDataRequester.validators;
//
// namespace ConsoleDataRequester.converters.console.utils;
//
// public static class BaseTypeArrayDataConverterFactory
// {
//     public static ConsoleArrayDataConverter<int>
//         MakeIntArrayConverter(IValidatableData<int>? nestedValidator = default, string delimiter = ";")
//     {
//         return new ConsoleArrayDataConverter<int>(BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
//             nestedValidator, delimiter);
//     }
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
// }