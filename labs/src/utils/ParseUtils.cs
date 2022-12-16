using System.Globalization;

namespace labs.utils;

public static class ParseUtils
{
    public static bool TryDoubleWithInvariant(string? data, out double value)
    {
        return double.TryParse(data, out value) 
               && double.TryParse(
                   data,
                   NumberStyles.Float | NumberStyles.AllowThousands,
                   NumberFormatInfo.InvariantInfo, out value
                );
    }

    public static bool TryInt(string? data, out int value)
    {
        return int.TryParse(data, out value);
    }
}