namespace labs.shared.data.abstracts.graph;

public sealed class GraphVertex : IComparable
{
    public object? Data { get; }

    public string ShortName { get; }

    public GraphVertex(string shortName, object? data = default)
    {
        Data = data;
        ShortName = shortName;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(obj, null)) return 1;
        if (ReferenceEquals(obj, this)) return 0;

        if (obj is not GraphVertex value)
            throw new ArgumentException();

        return string.Compare(ShortName, value.ShortName, StringComparison.Ordinal);
    }
}