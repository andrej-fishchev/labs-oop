using UserDataRequester.responses;

namespace UserDataRequester.validators;

public interface IValidatableData
{
    public bool Valid(IResponsibleData<object> responsibleData);
}