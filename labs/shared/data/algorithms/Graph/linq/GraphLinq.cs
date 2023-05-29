using labs.shared.data.abstracts.graph;

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
                matrix.SetLink(i, j, adj.Contains(vertexes[j]));
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
                if (matrix.GetLink(i, j))
                    graph.AddEdge(vertexes[i].ShortName, vertexes[j].ShortName);

        return graph;
    }
}