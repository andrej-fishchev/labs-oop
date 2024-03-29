using labs.shared.data.abstracts.graph;
using labs.shared.data.algorithms.Graph.searches;

namespace labs.shared.data.algorithms.Graph.linq;

public static class GraphLinq
{
    public static GraphMatrix ToMatrix(this abstracts.Graph graph)
    {
        if (graph.IsEmpty())
            return new GraphMatrix(new List<GraphVertex>());

        var vertexes = graph.Adjacency
                .Select(x => x.Vertex).ToList();

        vertexes.Sort();
        
        var matrix = new GraphMatrix(vertexes);

        for (int i = 0; i < graph.VertexCount; i++)
        {
            var adj = graph.Adjacency[
                graph.Adjacency.FindIndex(x => x.Vertex == vertexes[i])
            ].Keys;

            for (int j = 0; j < graph.VertexCount; j++)
                matrix.SetRelation(i, j, Convert.ToInt32(adj.Contains(vertexes[j])));
        }

        return matrix;
    }

    public static abstracts.Graph ToGraph(this GraphMatrix matrix)
    {
        if (matrix.VertexCount == 0)
            return new abstracts.Graph();

        var vertexes = matrix.GetVertexes();
        
        var graph = new abstracts.Graph();

        foreach (var vertex in vertexes)
            graph.AddVertex(vertex.ShortName, vertex.Data);
        
        for(int i = 0; i < graph.VertexCount; i++)
            for(int j = 0; j < graph.VertexCount; j++)
                if (matrix.GetRelation(i, j) != 0)
                    graph.AddEdge(vertexes[i].ShortName, vertexes[j].ShortName);

        return graph;
    }

    public static IEnumerable<GraphVertex> StreamVertexes(this GraphMatrix matrix) => 
        matrix.GetVertexes();

    public static IEnumerable<(int i, int j, int value)> StreamEdges(this GraphMatrix matrix)
    {
        if(matrix.VertexCount == 0)
            yield break;
        
        for(int i = 0, size = matrix.VertexCount; i < size; i++)
            for (int j = 0; j < size; j++)
                yield return (i, j, matrix.GetRelation(i, j));
    }

    public static IEnumerable<TO> Search<T, TO>(this GraphMatrix? graphMatrix, T algo) 
        where T : IGraphSearchAlgorithm<TO>
    {
        if(graphMatrix == null)
            yield break;

        foreach (var tuple in algo.Search(graphMatrix)) 
            yield return tuple;
    }
    
}