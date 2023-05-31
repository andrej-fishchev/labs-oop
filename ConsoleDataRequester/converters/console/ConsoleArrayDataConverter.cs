using System.Collections;
using UserDataRequester.responses;
using UserDataRequester.responses.console;
using UserDataRequester.validators;

namespace UserDataRequester.converters.console;

public class ConsoleArrayDataConverter :
    IConvertibleData
{
    public string Delimiter { get; set; }
    
    public IValidatableData? Validator { get; set; }
    
    public IConvertibleData? Converter { get; set; }
    
    public ConsoleArrayDataConverter(
        string delimiter = ",", 
        IConvertibleData? converter = default, 
        IValidatableData? validator = default)
    {
        Converter = converter;
        Delimiter = delimiter;
        Validator = validator;
    }
    
    public IResponsibleData<object> Convert(IResponsibleData<object> responsibleData)
    {
        if (!responsibleData.IsOk() || responsibleData.Data() == null)
            return responsibleData;

        if (responsibleData.Data() is not string buffer)
            return ConsoleResponseDataFactory.MakeResponse<object>(
                code: ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.UnableToConvertData));

        
        ArrayList output = new ArrayList();
        string[] entries = buffer.Split(Delimiter);
        ConsoleResponseData<object> response = new ConsoleResponseData<object>();

        foreach (var entry in entries)
        {
            response.Data(entry);
            response.StatusCode(ResponseStatusCode.ResponseOk);
            
            if(Converter != null && !Converter.Convert(response).IsOk())
                continue;
            
            if(Validator != null && !Validator.Valid(response))
                continue;

            output.Add(response.Data());
        }
        
        return ConsoleResponseDataFactory.MakeResponse<object>(output);
    }
}