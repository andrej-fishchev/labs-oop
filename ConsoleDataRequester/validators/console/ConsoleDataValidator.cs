using UserDataRequester.responses;
using UserDataRequester.validators.delegates;

namespace UserDataRequester.validators.console;

public class ConsoleDataValidator :
    IValidatableData,
    ICloneable
{
    public Validator Validator { get; set; }
    
    public ConsoleDataValidator(Validator fValidator) => 
        Validator = fValidator;

    public bool Valid(IResponsibleData<object> responsibleData) => 
        responsibleData.IsOk() && Validator.Invoke(responsibleData.Data());

    public object Clone() => new ConsoleDataValidator(Validator);
    
    public static implicit operator ConsoleDataValidator(Validator expression) =>
        new(expression);
}