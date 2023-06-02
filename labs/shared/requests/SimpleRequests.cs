using UserDataRequester.converters.console.utils;
using UserDataRequester.converters.parsers;
using UserDataRequester.requests.console.utils;
using UserDataRequester.validators.console.utils;

namespace labs.shared.requests;

public static class SimpleRequests
{
    public static int? GetValueInRangeNotStrict(int from, int to, string what = "", string terminate = "...")
    {
        var response = BaseDataTypeRequester.RequestInt(
            what,
            BaseComparableValidatorFactory.MakeInRangeNotStrictValidator(from, to),
            terminate
        );

        if (!response.IsOk() || response.Data() is not int value)
            return null;
        
        return value;
    }

    public static DateOnly? GetDate(string what = "", string terminate = "...")
    {
        var response = ConsoleBaseRequester.RepeatableGetApprovedData(
            what,
            BaseDataConverterFactory.MakeObjectConverter(BaseParser.TryParseSignature<DateOnly>()),
            null,
            terminate
        );

        if (!response.IsOk() || response.Data() is not DateOnly value)
            return null;

        return value;
    }
}