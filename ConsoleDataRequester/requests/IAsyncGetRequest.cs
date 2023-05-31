using UserDataRequester.responses;

namespace UserDataRequester.requests;

public interface IAsyncGetRequest
{
    public Task<IResponsibleData<object>> AsyncGet(string what);
}