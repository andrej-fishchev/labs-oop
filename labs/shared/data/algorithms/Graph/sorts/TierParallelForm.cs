using System.Collections;
using labs.shared.data.abstracts.graph;

namespace labs.shared.data.algorithms.Graph.sorts;

public static class TierParallelForm
{
    public static IEnumerable<(int level, GraphVertex)> TopologicalSort(this GraphMatrix matrix)
    {
        if(matrix.VertexCount == 0)
            yield break;

        var table = matrix.GetBitMatrixContainer();

        var level = 0;
        List<int> nullColumns;
        var alreadyNull = new List<int>();
        do
        {
            nullColumns = GetNullableColumns(table, matrix.VertexCount);
                
            foreach (var column in (nullColumns = nullColumns.Except(alreadyNull).ToList()))
                yield return (level, matrix.GetVertex(column));

            if(nullColumns.Count == 0 && alreadyNull.Count != matrix.VertexCount)
                yield break;
            
            alreadyNull.AddRange(nullColumns);

            NullTableRows(ref table, nullColumns, matrix.VertexCount);

            level++;
        }
        while (alreadyNull.Count != matrix.VertexCount && level < matrix.VertexCount);
    }

    private static List<int> GetNullableColumns(IReadOnlyList<BitArray> table, int size)
    {
        var list = new List<int>();

        for (int i = 0, a; i < size; i++)
        {
            var buffer = false;
            
            for (int j = 0; j < size && buffer == false; j++)
                buffer = table[j][i];
            
            if(!buffer)
                list.Add(i);
        }

        return list;
    }

    private static void NullTableRows(ref BitArray[] table, IReadOnlyList<int> columns, int size)
    {
        for(int i = 0; i < columns.Count; i++)
            for (int j = 0; j < size; j++)
                table[columns[i]][j] = false;
    }
}