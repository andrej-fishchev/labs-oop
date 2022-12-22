using labs.IO;

namespace labs.factories;

public static class ConsoleDataRequestFactory<T>
{
    public static ConsoleDataRequest<T> MakeConsoleDataRequest(ConsoleDataConverter<T> converter, 
        string message = "", 
        string reject = "...")
    {
        return new ConsoleDataRequest<T>(message, converter, reject);
    }

    public static ConsoleDataRequest<T> MakeConsoleDataRequestFromSelf(ConsoleDataRequest<T> self, string? message = default)
    {
        ConsoleDataRequest<T> cloned = (ConsoleDataRequest<T>)
            self.Clone();

        if (message != null)
            cloned.DisplayMessage = message;

        return cloned;
    }
}