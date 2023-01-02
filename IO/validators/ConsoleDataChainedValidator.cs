using IO.responses;

namespace IO.validators;

public class ConsoleDataChainedValidator<T> :
    IValidatableData<T>
{ 
    public IList<IValidatableData<T>> Validators { get; set; }

    public ConsoleDataChainedValidator(IList<IValidatableData<T>> validators)
    {
        Validators = validators;
    }

    public IResponsibleData<T> Validate(IResponsibleData<T> responsibleData)
    {
        ConsoleResponseData<T> buffer = responsibleData.As<ConsoleResponseData<T>>();
        
        using (IEnumerator<IValidatableData<T>> enumerator = Validators.GetEnumerator())
            while (enumerator.MoveNext())
                if ((responsibleData = enumerator.Current.Validate(responsibleData)).Error() != null)
                    return responsibleData;

        buffer |= ConsoleResponseDataCode.ConsoleOk;
        buffer |= String.Empty;
        
        return buffer;
    }

    public static ConsoleDataChainedValidator<T> 
        operator +(ConsoleDataChainedValidator<T> obj, IValidatableData<T> validator)
    {
        obj.Validators.Add(validator);
        return obj;
    }

    public static ConsoleDataChainedValidator<T>
        operator +(ConsoleDataChainedValidator<T> obj, IList<IValidatableData<T>> validators)
    {
        obj.Validators = obj.Validators.Concat(validators).ToList();
        return obj;
    }
}