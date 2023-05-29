using labs.shared.data.abstracts.graph;

namespace labs.shared.data.abstracts;

public class Graph
{
    public List<GraphVertexEdgeDictionary> Adjacency { get; }

    public int VertexCount => Adjacency.Count;

    public Graph(List<GraphVertexEdgeDictionary>? adjacency = default)
    {
        Adjacency = adjacency ?? new List<GraphVertexEdgeDictionary>();
    }

    public virtual bool IsEmpty() => VertexCount == 0;

    protected int FindVertex(Predicate<GraphVertex> predicate)
    {
        int pos = -1;
        for (int i = 0; i < VertexCount && pos == -1; i++)
        {
            if (predicate.Invoke(Adjacency[i].Vertex))
                pos = i;
        }
        
        return pos;
    }
    
    public virtual bool AddVertex(string shortName, object? data)
    {
        if (FindVertex(ShortNameEquals(shortName)) != -1)
            return false;

        Adjacency.Add(new GraphVertexEdgeDictionary(new GraphVertex(shortName, data)));
        
        return true;
    }

    public virtual void RemoveVertex(string shortName) => 
        RemoveVertex(ShortNameEquals(shortName));

    public virtual void RemoveVertex(Predicate<GraphVertex> condition)
    {
        int index;
        if((index = FindVertex(condition)) == -1)
            return;

        var vertex = Adjacency[index].Vertex;
        
        Adjacency.RemoveAt(index);
        Adjacency.ForEach(x => x.Remove(vertex));
    }

    public virtual bool AddEdge(string shortNameFrom, string shortNameTo, object? weight = default)
    {
        int index;
        if ((index = FindVertex(ShortNameEquals(shortNameFrom))) == -1)
            return false;

        int targetIdx;
        if ((targetIdx = FindVertex(ShortNameEquals(shortNameTo))) == -1)
            return false;
        
        Adjacency[index].Add(Adjacency[targetIdx].Vertex, weight);
        
        return true;
    }

    public virtual void RemoveEdge(string shortNameFrom, string shortNameTo)
    {
        int index;
        if ((index = FindVertex(ShortNameEquals(shortNameFrom))) == -1)
            return;
        
        int targetIdx;
        if ((targetIdx = FindVertex(ShortNameEquals(shortNameTo))) == -1)
            return;
        
        Adjacency[index].Remove(Adjacency[targetIdx].Vertex);
    }

    public virtual bool HasRelation(string shortNameFrom, string shortNameTo)
    {
        int index;
        if ((index = FindVertex(ShortNameEquals(shortNameFrom))) == -1)
            return false;
        
        int targetIdx;
        return (targetIdx = FindVertex(ShortNameEquals(shortNameTo))) != -1 
               && Adjacency[index].ContainsKey(Adjacency[targetIdx].Vertex);
    }

    public void Clear() => Adjacency.Clear();

    protected Predicate<GraphVertex> ShortNameEquals(string value) => 
        vertex => vertex.ShortName == value;
}