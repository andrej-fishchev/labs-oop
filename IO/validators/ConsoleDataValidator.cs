using IO.responses;

namespace IO.validators;

public class ConsoleDataValidator<T> :
    IValidatableData<T>,
    ICloneable
{
    public string ErrorMessage { get; set; }
    
    public DataValidator<T> Validator { get; set; }

    public ConsoleDataValidator(DataValidator<T> ioValidator, string errorMessage = "")
    {
        Validator = ioValidator;
        ErrorMessage = errorMessage;
    }

    public IResponsibleData<T> Validate(IResponsibleData<T> responsibleData)
    {
        if (responsibleData.Data == null || !Validator.Invoke(responsibleData.Data))
            responsibleData.Error = ErrorMessage;

        return responsibleData;
    }

    public object Clone()
    {
        return new ConsoleDataValidator<T>(Validator, ErrorMessage);
    }
}