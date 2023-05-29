using labs.shared.data.abstracts.graph;

namespace labs.shared.data.algorithms.Graph.walks;

public sealed class BreadthFirstWalk : IGraphWalkAlgorithm
{
    public IEnumerable<GraphVertex> Walk(abstracts.Graph graph, string vertexShortName)
    {
        var adjacency = graph.Adjacency;

        int index;
        if(adjacency.Count == 0 || (index = adjacency
               .FindIndex(x => x.Vertex.ShortName == vertexShortName)) == -1)
            yield break;
        
        var visited = new bool[adjacency.Count];
        var queue = new Queue<int>();
        
        visited[index] = true;
        queue.Enqueue(index);

        while (queue.Count != 0)
        {
            index = queue.Dequeue();

            yield return adjacency[index].Vertex;

            for (int i = 0, a; i < adjacency[index].Count; i++)
            {
                a = adjacency.FindIndex(x => 
                    x.Vertex.ShortName == adjacency[index].Keys.ToArray()[i].ShortName);
                
                if (visited[a]) 
                    continue;
                
                visited[a] = true;
                queue.Enqueue(a);
            }
        }
    }
}