using UserDataRequester.responses;
using UserDataRequester.validators.delegates;

namespace UserDataRequester.validators.console;

public class ConsoleDataChainedValidator :
    IValidatableData
{
    public IList<IValidatableData> Validators { get; set; }
    
    public ConsoleDataChainedValidator(IList<IValidatableData>? validators = default) =>
        Validators = validators ?? new List<IValidatableData>();
    
    public bool Valid(IResponsibleData<object> responsibleData)
    {
        foreach (var validator in Validators)
            if (!validator.Valid(responsibleData))
                return false;

        return true;
    }

    public ConsoleDataChainedValidator And(IValidatableData validator)
    {
        Validators.Add(validator);
        return this;
    }
    
    public ConsoleDataChainedValidator And(Validator expression)
    {
        Validators.Add(new ConsoleDataValidator(expression));
        return this;
    }

    public static ConsoleDataChainedValidator operator +(ConsoleDataChainedValidator obj, IValidatableData validator) 
        => obj.And(validator);

    public static ConsoleDataChainedValidator operator +(ConsoleDataChainedValidator obj, IList<IValidatableData> validators)
    {
        obj.Validators = obj.Validators.Concat(validators).ToList();
        return obj;
    }
}