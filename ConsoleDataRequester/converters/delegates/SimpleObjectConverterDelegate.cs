using System.Reflection;

namespace UserDataRequester.converters.delegates;

public delegate bool SimpleObjectConverterDelegate(MethodInfo? parserSig, object? data, out object output);