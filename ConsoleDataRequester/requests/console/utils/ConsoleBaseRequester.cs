using UserDataRequester.converters;
using UserDataRequester.responses;
using UserDataRequester.responses.console;
using UserDataRequester.validators;
using UserDataRequester.validators.console;

namespace UserDataRequester.requests.console.utils;

public static class ConsoleBaseRequester
{
    public static IResponsibleData<object> RepeatableGetApprovedData(
        string what, 
        IConvertibleData? converter = default,
        IValidatableData? validator = default, 
        string? terminateString = "...")
    {
        ConsoleResponseData<object> response;
        
        var req = new ConsoleDataRequest();

        do
        {
            if (!(response = req.Get(what).As<ConsoleResponseData<object>>()).IsOk())
                return response;

            if (terminateString != null && response.Data()!.Equals(terminateString))
            {
                response.StatusCode(ResponseStatusCodeFactory.Create(ConsoleDataCode.ConsoleInputRejected));
                return response;
            }
            
            if (response.IsOk() && converter != null)
                if(!(response = converter.Convert(response).As<ConsoleResponseData<object>>()).IsOk())
                    Console.Error.WriteLine("Не удалось преобразовать значение, повторите ввод");

            if (response.IsOk() && validator != null && !validator.Valid(response))
            {
                response.StatusCode(ResponseStatusCodeFactory.Create(ConsoleDataValidatorCode.InvalidInputObjectData));
                
                Console.Error.WriteLine("Ввод не удовлетворяет условиям, повторите ввод");
            }
            
        } while (!response.IsOk());
        
        return response;
    }
}