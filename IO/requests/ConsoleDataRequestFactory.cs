namespace IO.requests;

public static class ConsoleDataRequestFactory
{
    public static ConsoleDataRequest<T> MakeConsoleDataRequest<T>(string message = "", string reject = "...")
    {
        return new ConsoleDataRequest<T>(message, reject);
    }

    public static ConsoleDataRequest<T> MakeConsoleDataRequestFromSelf<T>(ConsoleDataRequest<T> self, string? message = default)
    {
        ConsoleDataRequest<T> cloned = (ConsoleDataRequest<T>)
            self.Clone();

        if (message != null)
            cloned.DisplayMessage = message;

        return cloned;
    }
}