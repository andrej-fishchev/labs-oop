using labs.shared.data.abstracts.graph;
using labs.shared.data.algorithms.Graph.walks.delegates;

namespace labs.shared.data.algorithms.Graph.walks.linq;

public static class GraphWalkLinq
{
    public static IEnumerable<GraphVertex> Walk(
        this abstracts.Graph graph, 
        IGraphWalkAlgorithm algorithm, 
        string vertexStart) => 
        algorithm.Walk(graph, vertexStart);
    
    public static IEnumerable<GraphVertex> Walk(
        this abstracts.Graph graph, 
        GraphWalkDelegate algorithm, 
        string vertexStart) => 
        algorithm.Invoke(graph, vertexStart);
}