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

        ConsoleResponseData<T> buffer;
        (buffer = responsibleData.As<ConsoleResponseData<T>>())
            .Error(error);
        
        return buffer;
    }

    public static implicit operator ConsoleDataValidator<T>((Validator<T> expression, string error) obj) 
        => new(obj.expression, obj.error);

    public object Clone() => new ConsoleDataValidator<T>(Validator, ErrorMessage);
}