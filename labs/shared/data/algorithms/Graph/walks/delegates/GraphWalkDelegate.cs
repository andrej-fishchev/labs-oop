using labs.shared.data.abstracts.graph;

namespace labs.shared.data.algorithms.Graph.walks.delegates;

public delegate IEnumerable<GraphVertex> GraphWalkDelegate(abstracts.Graph graph, string vertexStart);