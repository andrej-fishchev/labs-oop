using System.Collections;
using System.Text;
using labs.shared.data.algorithms.Graph.linq;

namespace labs.shared.data.abstracts.graph;

public class GraphMatrix : ICloneable
{
    private int[,] matrix;

    private readonly IList<GraphVertex> vertexes;

    public GraphMatrix(IList<GraphVertex> vertexes)
    {
        this.vertexes = vertexes;
        
        matrix = new int[this.vertexes.Count, this.vertexes.Count];
    }

    private GraphMatrix(IList<GraphVertex> vertexes, int[,] container)
    {
        this.vertexes = vertexes;
        matrix = container;
    }

    public static GraphMatrix E(IList<GraphVertex> vertexes)
    {
        var x = new GraphMatrix(vertexes);
        
        for(int i = 0; i < x.VertexCount; i++)
            x.SetRelation(i, i, 1);

        return x;
    }

    public int VertexCount => vertexes.Count;
    
    public void SetRelation(int from, int to, int value) => matrix[from,to] = value;

    public int GetRelation(int from, int to) => matrix[from,to];

    public GraphVertex GetVertex(int idx) => vertexes[idx];

    public GraphMatrix AddVertex(GraphVertex value)
    {
        if(vertexes.Contains(value))
            return this;

        var newVertexes = GetVertexes();
        
        newVertexes.Add(value);

        var buffer = new GraphMatrix(newVertexes);
        
        for(int i = 0; i < vertexes.Count; i++)
            for(int j = 0; j < vertexes.Count; j++)
                buffer.SetRelation(i, j, GetRelation(i, j));

        return buffer;
    }

    public static GraphMatrix? Multiply(GraphMatrix l, GraphMatrix r)
    {
        if (l.VertexCount != r.VertexCount)
            return null;

        var result = new GraphMatrix(l.GetVertexes());

        for (int i = 0; i < l.VertexCount; i++)
        {
            for (int j = 0; j < l.VertexCount; j++)
            {
                for (int k = 0; k < l.VertexCount; k++)
                {
                    result.SetRelation(i, j,
                        result.GetRelation(i, j) + l.GetRelation(i, k) * r.GetRelation(k, j));
                }
            }
        }

        return result;
    }
    
    // 6-010000-110000-000110-001010-000100-011011
    public static GraphMatrix Or(GraphMatrix l, GraphMatrix r)
    {
        var result = new GraphMatrix(l.GetVertexes());

        for (int i = 0; i < l.VertexCount; i++)
        {
            for (int j = 0; j < l.VertexCount; j++)
            {
                result.SetRelation(i, j, 
                    l.GetRelation(i, j) & r.GetRelation(i,j)
                );
            }
        }

        return result;
    }

    public static GraphMatrix Sum(GraphMatrix l, GraphMatrix r)
    {
        var result = new GraphMatrix(l.GetVertexes());

        for (int i = 0; i < l.VertexCount; i++)
        {
            for (int j = 0; j < l.VertexCount; j++)
            {
                result.SetRelation(i, j, l.GetRelation(i, j) + r.GetRelation(i,j));
            }
        }

        return result;
    }

    public static GraphMatrix Transposition(GraphMatrix matrix)
    {
        if (matrix.VertexCount == 0)
            return matrix;

        var buffer = (GraphMatrix) matrix.Clone();
        
        for (int i = 0; i < buffer.VertexCount; i++)
        {
            for (int j = 0; j < buffer.VertexCount; j++)
            {
                buffer.SetRelation(i, j, matrix.GetRelation(j, i));
            }
        }

        return buffer;
    }

    public BitArray[] GetBitMatrixContainer()
    {
        BitArray[] copy = new BitArray[VertexCount];

        for(int i = 0; i < VertexCount; i++)
        {
            copy[i] = new BitArray(VertexCount);

            for (int j = 0; j < VertexCount; j++)
                copy[i][j] = Convert.ToBoolean(matrix[i,j]);
        }
        
        return copy;
    }

    public int[,] GetMatrixContainer()
    {
        int[,] container = new int[VertexCount, VertexCount];
        
        for(int i = 0; i < VertexCount; i++)
            for (int j = 0; j < VertexCount; j++)
                container[i, j] = GetRelation(i, j);

        return container;
    }

    public IList<GraphVertex> GetVertexes() => 
        (IList<GraphVertex>) vertexes.ToArray().Clone();

    public override string ToString()
    {
        var builder = new StringBuilder($"{VertexCount}");
        
        if (VertexCount == 0)
            return builder.ToString();

        for (int i = 0; i < VertexCount; i++)
        {
            builder.Append('-');

            for (int j = 0; j < VertexCount; j++)
                builder.Append(Convert.ToInt32(Convert.ToBoolean(GetRelation(i, j))));
        }

        return builder.ToString();
    }
    
    public bool HasLoops()
    {
        if (VertexCount == 0)
            return false;
        
        for(int i = 0; i < VertexCount; i++)
            if (GetRelation(i, i) != 0)
                return true;

        return false;
    }

    public GraphMatrix? GetReachabilityMatrix()
    {
        var bitMatrix = GetBitMatrixContainer();

        var size = bitMatrix.Length;
        
        if (size == 0)
            return null;

        var buffer = new GraphMatrix(GetVertexes());

        for (var k = 0; k < vertexes.Count; k++)
        {
            for (int i = 0; i < vertexes.Count; i++)
            {
                for (int j = 0; j < vertexes.Count; j++)
                { 
                    buffer.SetRelation(i, j, 
                        Convert.ToInt32(bitMatrix[i][j] || (bitMatrix[i][k] && bitMatrix[k][j])));
                }
            }
        }

        return buffer;
    }

    public GraphMatrix RemoveLoops()
    {
        var clone = (GraphMatrix)Clone();
        
        for(int i = 0; i < clone.VertexCount; i++)
            clone.SetRelation(i, i, 0);

        return clone;
    }

    public object Clone()
    {
        return new GraphMatrix(GetVertexes(), GetMatrixContainer());
    }
    
    public static bool TryParse(object? data, out GraphMatrix output)
    {
        output = new GraphMatrix(new List<GraphVertex>());

        if (data is not string value)
            return false;

        string[] buffer = value.Split("-");

        if (buffer.Length == 0)
            return false;

        if (!int.TryParse(buffer[0], out var vertexCount) || vertexCount < 1)
            return false;

        if (buffer.Length != vertexCount + 1)
            return false;
        
        output = new GraphMatrix(Enumerable.Range(0, vertexCount)
            .Select(x => new GraphVertex(((char)(x + 65)).ToString())).ToList());

        for (int i = 0; i < vertexCount; i++)
        {
            if (buffer[i + 1].Length != vertexCount || buffer[i + 1].ToCharArray() is not { } chain)
                return false;

            for (int j = 0; j < vertexCount; j++)
            {
                if (!int.TryParse(new ReadOnlySpan<char>(new[] { chain[j] }), out var relation))
                    return false;
                
                output.SetRelation(i, j, Convert.ToInt32(Convert.ToBoolean(relation)));
            }
        }

        return true;
    }
}