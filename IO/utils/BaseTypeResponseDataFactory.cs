using IO.converters;
using IO.requests;
using IO.responses;
using IO.validators;

namespace IO.utils;

public static class BaseTypeResponseDataFactory
{
    public static ConsoleResponseData<T[]>[] MakeRequest<T>(uint size,
        IRequestableData<string?, T[]> instance,
        IConvertibleData<string?, T[]> converter,
        IValidatableData<T[]>? validator = default,
        bool sendRejectMessage = false)
    {
        ConsoleResponseData<T[]>[] response = 
            new ConsoleResponseData<T[]>[size];

        for (int i = 0; i < response.Length; i++)
        {
            response[i] = (ConsoleResponseData<T[]>) 
                instance.Request(converter, validator, sendRejectMessage);

            if (response[i].Code != (int)ConsoleResponseDataCode.ConsoleOk)
                return new[]
                {
                    response[i]
                };
        }

        return response;
    }
}