using IO.responses;

namespace IO.validators;

public interface IValidatableData<T>
{
    public IResponsibleData<T> Validate(IResponsibleData<T> responsibleData);
}
    