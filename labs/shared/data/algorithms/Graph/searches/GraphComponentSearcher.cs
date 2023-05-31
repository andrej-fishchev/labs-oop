using System.Collections;
using labs.shared.data.abstracts.graph;

namespace labs.shared.data.algorithms.Graph.searches;

public static class GraphComponentSearcher
{
    public static IEnumerable<List<GraphVertex>> Components(this GraphMatrix matrix)
    {
        if(matrix.VertexCount == 0)
            yield break;
        
        var table = PrepareFinalMatrix(matrix).GetBitMatrixContainer();

        foreach (var componenta in GetValidColumns(
                     table, matrix.GetVertexes().ToList(), matrix.VertexCount))
            yield return componenta;
    }

    private static GraphMatrix PrepareFinalMatrix(GraphMatrix matrix)
    {
        var mtrs = new List<GraphMatrix> {matrix};
        for(int i = 1; i < matrix.VertexCount; i++)
            mtrs.Add(GraphMatrix.Multiply(mtrs[i - 1], matrix)!);
        
        var output = GraphMatrix.E(matrix.GetVertexes());
        for (int i = 0; i < mtrs.Count; i++)
            output = GraphMatrix.Sum(output, mtrs[i]);

        return GraphMatrix.Or(output, GraphMatrix.Transposition(output));
    }

    private static IEnumerable<List<GraphVertex>> GetValidColumns(BitArray[] table, List<GraphVertex> v, int size)
    {
        var line = new List<int>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(table[i][j])
                    line.Add(j);
            }
                
            if(line.Count == 0)
                continue;

            yield return line.Select(x => v[x]).ToList();

            table = ExcludeVertexes(table, line, size);
            
            line.Clear();
        }
    }

    private static BitArray[] ExcludeVertexes(BitArray[] table, IReadOnlyList<int> vertexes, int size)
    {
        foreach (var t in vertexes)
            for (int j = 0; j < size; j++)
                table[t][j] = false;

        return table;
    }
}