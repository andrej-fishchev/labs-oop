namespace labs.utils;

public static class DataConverterUtils
{
    private static string failMessagePattern = 
        "ожидалось {type} число, но получено '{value}'";

    public static string UseMessagePattern(string type, string value)
    {
        return failMessagePattern
            .Replace("{type}", type)
            .Replace("{value}", value);
    }

    public static double ToDoubleWithInvariant(string? data, out string? error)
    {
        error = null;

        if (!ParseUtils.TryDoubleWithInvariant(data, out var value))
            error = UseMessagePattern("вещественное", $"{data}");

        return value;
    }
    
    public static int ToInt(string? data, out string? error)
    {
        error = null;

        if (!ParseUtils.TryInt(data, out var value))
            error = UseMessagePattern("целое", $"{data}");

        return value;
    }
}