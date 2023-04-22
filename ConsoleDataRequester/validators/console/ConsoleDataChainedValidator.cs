using UserDataRequester.responses;

namespace UserDataRequester.validators.console;

public class ConsoleDataChainedValidator :
    IValidatableData
{
    public IList<IValidatableData> Validators { get; set; }
    
    public ConsoleDataChainedValidator(IList<IValidatableData> validators) =>
        Validators = validators;
    
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

    public static ConsoleDataChainedValidator operator +(ConsoleDataChainedValidator obj, IValidatableData validator) 
        => obj.And(validator);

    public static ConsoleDataChainedValidator operator +(ConsoleDataChainedValidator obj, IList<IValidatableData> validators)
    {
        obj.Validators = obj.Validators.Concat(validators).ToList();
        return obj;
    }
}