using labs.shared.data.abstracts.graph;
using UserDataRequester.converters.console;
using UserDataRequester.converters.console.utils;
using UserDataRequester.converters.parsers;

namespace labs.shared.data.requests.graph.converters;

public static class GraphMatrixConverter
{
    public static ConsoleSimpleDataConverter MakeSimpleInlineGraphMatrixConverter() =>
        BaseDataConverterFactory.MakeObjectConverter(BaseParser.TryParseSignature<GraphMatrix>());
}