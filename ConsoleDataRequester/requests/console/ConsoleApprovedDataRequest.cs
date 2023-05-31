using UserDataRequester.converters;
using UserDataRequester.responses;
using UserDataRequester.responses.console;
using UserDataRequester.validators;
using UserDataRequester.validators.console;

namespace UserDataRequester.requests.console;

public class ConsoleApprovedDataRequest :
    IGetRequest,
    IAsyncGetRequest
{
    public static readonly ConsoleDataRequest RequestObject = new();
    
    public IConvertibleData Converter { get; set; }

    public IValidatableData? Validator { get; set; }

    public ConsoleApprovedDataRequest(IConvertibleData converter, IValidatableData? validator = default)
    {
        Converter = converter;
        Validator = validator;
    }
    
    public IResponsibleData<object> Get(string requestableData)
    {
        var buffer = RequestObject.Get(requestableData)
            .As<ConsoleResponseData<object>>();

        if (!buffer.IsOk())
            return ConsoleResponseDataFactory.MakeResponse<object>(code: buffer.StatusCode());

        ConsoleResponseData<object> output = Converter.Convert(buffer)
            .As<ConsoleResponseData<object>>();

        if (output.IsOk() && Validator != null && !Validator.Valid(output))
            output.StatusCode(ResponseStatusCodeFactory.Create(ConsoleDataValidatorCode.InvalidInputObjectData));
        
        return output;
    }

    public async Task<IResponsibleData<object>> AsyncGet(string requestableData)
    {
        var buffer = (await RequestObject.AsyncGet(requestableData))
            .As<ConsoleResponseData<object>>();

        if (!buffer.IsOk())
            return ConsoleResponseDataFactory.MakeResponse<object>(code: buffer.StatusCode());

        ConsoleResponseData<object> output = Converter.Convert(buffer)
            .As<ConsoleResponseData<object>>();

        if (output.IsOk() && Validator != null && !Validator.Valid(output))
            output.StatusCode(ResponseStatusCodeFactory.Create(ConsoleDataValidatorCode.InvalidInputObjectData));
        
        return output;
    }
}