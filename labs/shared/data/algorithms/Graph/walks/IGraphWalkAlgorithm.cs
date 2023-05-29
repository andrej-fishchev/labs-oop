using labs.shared.data.abstracts.graph;

namespace labs.shared.data.algorithms.Graph.walks;

public interface IGraphWalkAlgorithm : IAlgorithm
{
    public IEnumerable<GraphVertex> Walk(abstracts.Graph graph, string vertexShortName);
}