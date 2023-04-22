using UserDataRequester.responses;

namespace UserDataRequester.converters;

public interface IConvertibleData
{
    public IResponsibleData<object> Convert(IResponsibleData<object> responsibleData);
}