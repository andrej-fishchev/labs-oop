using System.Collections;

namespace labs.shared.data.abstracts.graph;

public sealed class GraphVertexEdgeDictionary : IDictionary<GraphVertex, object?>
{
    public GraphVertex Vertex { get; }

    public IDictionary<GraphVertex, object?> Relations { get; }

    public GraphVertexEdgeDictionary(GraphVertex vertex, IDictionary<GraphVertex, object?>? relations = default)
    {
        Vertex = vertex;
        Relations = relations ?? new Dictionary<GraphVertex, object?>();
    }
    
    public IEnumerator<KeyValuePair<GraphVertex, object?>> GetEnumerator() =>
        Relations.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(KeyValuePair<GraphVertex, object?> item) =>
        Relations.Add(item);

    public void Clear() => Relations.Clear();

    public bool Contains(KeyValuePair<GraphVertex, object?> item) => 
        Relations.Contains(item);

    public void CopyTo(KeyValuePair<GraphVertex, object?>[] array, int arrayIndex) =>
        Relations.CopyTo(array, arrayIndex);

    public bool Remove(KeyValuePair<GraphVertex, object?> item) => 
        Relations.Remove(item);

    public int Count => Relations.Count;

    public bool IsReadOnly => Relations.IsReadOnly;

    public void Add(GraphVertex key, object? value) => Relations.Add(key, value);

    public bool ContainsKey(GraphVertex key) => Relations.ContainsKey(key);

    public bool Remove(GraphVertex key) => Relations.Remove(key);

    public bool TryGetValue(GraphVertex key, out object? value) => Relations.TryGetValue(key, out value);

    public object? this[GraphVertex key]
    {
        get => Relations[key];
        set => Relations[key] = value;
    }

    public ICollection<GraphVertex> Keys => Relations.Keys;

    public ICollection<object?> Values => Relations.Values;
}