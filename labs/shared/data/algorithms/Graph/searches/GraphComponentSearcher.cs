using labs.shared.data.abstracts.graph;
using labs.shared.data.algorithms.Graph.walks;
using labs.shared.data.algorithms.Graph.walks.linq;

namespace labs.shared.data.algorithms.Graph.searches;

public static class GraphComponentSearcher
{
    public static IEnumerable<List<GraphVertex>> Components(this abstracts.Graph graph)
    {
        if(graph.VertexCount == 0)
            yield break;
        
        var list = new List<GraphVertex>();
        for (var i = 0; i < graph.VertexCount; i++)
        {
            if(list.Contains(graph.Adjacency[i].Vertex))
                continue;

            var dfsResult = graph.Walk(
                new DepthFirstWalk(), graph.Adjacency[i].Vertex.ShortName
            ).ToList();
            
            yield return dfsResult;
            
            list.AddRange(dfsResult);
        }
    }
}