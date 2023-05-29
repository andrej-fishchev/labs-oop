using System.Collections;
using System.Text;

namespace labs.shared.data.abstracts.graph;

public sealed class GraphMatrix : ICloneable
{
    private BitArray[] matrix;

    private readonly IList<GraphVertex> vertexes;

    public GraphMatrix(IList<GraphVertex> vertexes)
    {
        this.vertexes = vertexes;
        
        matrix = new BitArray[this.vertexes.Count];

        for (var i = 0; i < matrix.Length; i++)
            matrix[i] = new BitArray(this.vertexes.Count);
    }

    public int VertexCount => vertexes.Count;
    
    public void SetLink(int from, int to, bool value) => 
        matrix[from][to] = value;

    public bool GetLink(int from, int to) => matrix[from][to];

    public GraphVertex GetVertex(int idx) => vertexes[idx];

    public GraphMatrix AddVertex(GraphVertex value)
    {
        if(vertexes.Contains(value))
            return this;

        var newVertexes = GetVertexes();
        newVertexes.Add(value);

        GraphMatrix buffer = new GraphMatrix(newVertexes);
        
        for(int i = 0; i < vertexes.Count; i++)
            for(int j = 0; j < vertexes.Count; j++)
                buffer.SetLink(i, j, GetLink(i, j));

        return buffer;
    }

    public BitArray[] GetInnerContainer() => 
        (BitArray[])matrix.Clone();
    
    public IList<GraphVertex> GetVertexes() => 
        (IList<GraphVertex>)vertexes.ToArray().Clone();

    public override string ToString()
    {
        if (vertexes.Count == 0)
            return "";
        
        StringBuilder builder = new StringBuilder();
        for (int i = -1; i < vertexes.Count; i++)
        {
            if (i == -1)
                builder.Append('\t');

            else builder.Append(vertexes[i].ShortName).Append('\t');
            
            for (int j = 0; j < vertexes.Count; j++)
            {
                if (i == -1)
                    builder.Append("|\t").Append(vertexes[j].ShortName).Append('\t');

                else
                    builder.Append("|\t").Append(Convert.ToInt32(matrix[i][j])).Append('\t');
            }

            builder.Append("|\n");
        }

        return builder.ToString();
    }
    
    public bool HasLoops()
    {
        if (VertexCount == 0)
            return false;
        
        for(int i = 0; i < VertexCount; i++)
            if (GetLink(i, i))
                return true;

        return false;
    }

    public GraphMatrix? GetReachabilityMatrix()
    {
        var vertexes = GetVertexes();

        if (vertexes.Count == 0)
            return null;

        GraphMatrix buffer = (GraphMatrix)Clone();

        bool a, b, c;
        
        for (int k = 0; k < vertexes.Count; k++)
        {
            for (int i = 0; i < vertexes.Count; i++)
            {
                for (int j = 0; j < vertexes.Count; j++)
                {
                    a = buffer.GetLink(i, j);
                    b = buffer.GetLink(i, k);
                    c = buffer.GetLink(k, j);
                    
                    buffer.SetLink(i, j, (a || (b && c)));
                }
            }
        }

        return buffer;
    }

    public GraphMatrix RemoveLoops()
    {
        GraphMatrix clone = (GraphMatrix)Clone();
        
        for(int i = 0; i < clone.VertexCount; i++)
            clone.SetLink(i, i, false);

        return clone;
    }

    public object Clone()
    {
        GraphMatrix clone = new GraphMatrix(GetVertexes());
        
        for(int i = 0; i < vertexes.Count; i++)
            for(int j = 0; j < vertexes.Count; j++)
                clone.SetLink(i, j, GetLink(i, j));

        return clone;
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
                
                output.SetLink(i, j, Convert.ToBoolean(relation));
            }
        }

        return true;
    }
}