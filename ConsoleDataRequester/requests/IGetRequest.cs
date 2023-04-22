using UserDataRequester.responses;

namespace UserDataRequester.requests;

public interface IGetRequest
{
    public IResponsibleData<object> Get(string what);
}