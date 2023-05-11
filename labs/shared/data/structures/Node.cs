namespace labs.shared.data.structures;

public class Node<T>
{
    private Node<T>? next;

    private Node<T>? prev;
    
    public T Data { get; set; }

    public Node(T data, Node<T>? prev = default, Node<T>? next = default)
    {
        Data = data;

        Next(next);
        Prev(prev);
    }

    public void Next(Node<T>? node)
    {
        next = node;

        if (!ReferenceEquals(null, next) && next.Prev() != this)
            next.Prev(this);
    }

    public void Prev(Node<T>? node)
    {
        prev = node;

        if (!ReferenceEquals(null, prev) && prev.Next() != this)
            prev.Next(this);
    }

    public Node<T>? Next() => next;

    public Node<T>? Prev() => prev;
    
    public static Node<T>? operator ++(Node<T>? self) => 
        self?.Next();

    public static Node<T>? operator --(Node<T>? self) =>
        self?.Prev();

    // public static bool operator ==(Node<T>? left, Node<T>? right)
    // {
    //     if (ReferenceEquals(left, right)) return true;
    //     
    //     return left.Equals((object?)right);
    // }

    // public static bool operator !=(Node<T>? left, Node<T>? right)
    // {
    //     return !(left == right);
    // }
}