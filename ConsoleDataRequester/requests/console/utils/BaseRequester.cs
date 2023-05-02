using UserDataRequester.converters;
using UserDataRequester.responses;
using UserDataRequester.responses.console;
using UserDataRequester.validators;
using UserDataRequester.validators.console;

namespace UserDataRequester.requests.console.utils;

public static class BaseRequester
{
    public static IResponsibleData<object> While(
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
            }
            
            if (response.IsOk() && converter != null)
                if(!(response = converter.Convert(response).As<ConsoleResponseData<object>>()).IsOk())
                    Console.Error.WriteLine("Не удалось выполнить преобразование типов");

            if (response.IsOk() && validator != null && !validator.Valid(response))
            {
                response.StatusCode(ResponseStatusCodeFactory.Create(ConsoleDataValidatorCode.InvalidInputObjectData));
                Console.Error.WriteLine("Ввод не удовлетворяет условиям");
            }
                

        } while (!response.IsOk());
        
        return response;
    }
}