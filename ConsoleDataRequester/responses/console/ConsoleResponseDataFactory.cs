namespace UserDataRequester.responses.console;

public static class ConsoleResponseDataFactory
{
    public static ConsoleResponseData<T> MakeResponse<T>(T? data = default, ResponseStatusCode? code = default)
    {
        return new ConsoleResponseData<T>(data, code ?? ResponseStatusCode.ResponseOk);
    }
}