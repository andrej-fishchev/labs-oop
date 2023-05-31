using System.Globalization;
using System.Reflection;

namespace UserDataRequester.converters.delegates;

public delegate bool SimpleNumberConverterDelegate(
    MethodInfo? parserSig, 
    object? data, 
    out object output,
    NumberStyles styles = NumberStyles.Any, 
    IFormatProvider? provider = default
);