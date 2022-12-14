using IO.responses;

namespace IO.validators;

public class ConsoleDataValidator<T> :
    IValidatableData<T>,
    ICloneable
{
    public string ErrorMessage { get; set; }
    
    public Validator<T> Validator { get; set; }

    public ConsoleDataValidator(Validator<T> ioValidator, string errorMessage = "")
    {
        Validator = ioValidator;
        ErrorMessage = errorMessage;
    }

    public IResponsibleData<T> Validate(IResponsibleData<T> responsibleData)
    {
        string error = Validator.Invoke(responsibleData.Data())
            ? String.Empty
            : ErrorMessage;

        ConsoleResponseData<T> buffer = responsibleData
            .As<ConsoleResponseData<T>>();

        buffer |= error;
        
        return buffer;
    }

    public object Clone()
    {
        return new ConsoleDataValidator<T>(Validator, ErrorMessage);
    }
}