using labs.shared.data.abstracts;
using labs.shared.data.abstracts.graph;
using labs.shared.data.requests.graph.converters;
using UserDataRequester.requests.console.utils;
using UserDataRequester.responses;
using UserDataRequester.validators;
using UserDataRequester.validators.console;

namespace labs.shared.data.requests.graph.requesters;

public static class GraphRequester
{
    public static GraphMatrix? GetGraphMatrix(string? terminate = "...")
    {
        Console.WriteLine($"\nВвод может быть прекращен в любой момент, используйте '{terminate}')\n");
        
        IResponsibleData<object> response;
        if (!(response = RequestVertex(terminate: terminate)).IsOk() || response.Data() is not int vCount)
            return null;

        var vertexes = Enumerable.Range(0, vCount)
            .Select(x => new GraphVertex(((char)(x + 65)).ToString())).ToList();

        var matrix = new GraphMatrix(vertexes);

        for (int i = 0; i < vertexes.Count; i++)
        {
            for (int j = 0; j < vertexes.Count; j++)
            {
                if (!(response = GetRelation(
                            $"Вес {vertexes[i].ShortName} и {vertexes[j].ShortName}: ",
                            terminate: terminate))
                        .IsOk() || response.Data() is not int value)
                    return null;
                
                matrix.SetRelation(i, j, value);
            }
        }

        return matrix;
    }
    
    public static GraphMatrix? GetGraphBitMatrix(string? terminate = "...")
    {
        Console.WriteLine($"\nВвод может быть прекращен в любой момент, используйте '{terminate}')\n");
        
        IResponsibleData<object> response;
        if (!(response = GetInlineMatrixString(
                "Введите матрицу в формате (кол.вершин-связи)\nПример: 3-001-100-111\n> ", 
                terminate)).IsOk() || response.Data() is not GraphMatrix matrix)
            return null;

        return matrix;
    }

    public static GraphVertex? GetVertex(Graph graph, string terminate = "...")
    {
        Console.WriteLine($"\nВвод может быть прекращен в любой момент, используйте '{terminate}')\n");
        
        for(int i = 0; i < graph.VertexCount; i++)
            Console.WriteLine($"{i + 1}. {graph.Adjacency[i].Vertex.ShortName}");
        
        IResponsibleData<object> response;
        if (!(response = GetInt("Выберите номер вершины, с которой необходимо начать обход: ", 
                terminate,
                new ConsoleDataChainedValidator()
                    .And(data => data != null)
                    .And(data => data is int value and > 0 && value <= graph.VertexCount)
                )).IsOk() 
            || response.Data() is not int vertexNumber)
            return null;

        return graph.Adjacency[--vertexNumber].Vertex;
    }

    public static IResponsibleData<object> GetInlineMatrixString(
        string msg = "",
        string? terminate = "...") =>
        RequestInlineGraphMatrix(msg, new ConsoleDataChainedValidator()
            .And(data => data != null)
            .And(data => data is GraphMatrix),
            terminate
        );

    public static IResponsibleData<object> GetRelation(
        string msg = "",
        string? terminate = "...") =>
        GetInt(msg, terminate, new ConsoleDataChainedValidator()
            .And(data => data != null)
            .And(data => data is int));

    public static IResponsibleData<object> RequestVertex(
        string msg = "Введите количество вершин: ",
        string? terminate = "...") =>
        GetInt(msg, terminate, new ConsoleDataChainedValidator()
            .And(data => data != null)
            .And(data => data is > 0));

    public static IResponsibleData<object> RequestInlineGraphMatrix(
        string what,
        IValidatableData? validator = default,
        string? terminateString = "..."
    ) => ConsoleBaseRequester.RepeatableGetApprovedData(what, 
        GraphMatrixConverter.MakeSimpleInlineGraphMatrixConverter(), 
        validator, 
        terminateString);

    
    public static IResponsibleData<object> GetInt(
        string message = "", 
        string? terminate = "...",
        IValidatableData? dataValidator = default) => 
        BaseDataTypeRequester.RequestInt(
        message,
        dataValidator,
        terminate);
}