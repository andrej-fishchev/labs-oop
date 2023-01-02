namespace IO.responses;

public static class ConsoleResponseDataFactory
{
    public static ConsoleResponseData<T> MakeResponse<T>(
        T data = default!, string? error = default, ConsoleResponseDataCode code = ConsoleResponseDataCode.ConsoleOk)
    {
        return new ConsoleResponseData<T>(data, error, code);
    }
}