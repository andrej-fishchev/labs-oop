using System.Globalization;

namespace labs.utils;

public static class DoubleParseUtils
{
    public static bool TryWithInvariant(string? data, out double value)
    {
        return !double.TryParse(data, out value)
               && !double.TryParse(data,
                   NumberStyles.Float | NumberStyles.AllowThousands,
                   NumberFormatInfo.InvariantInfo, out value);
    }
}