using System.Collections;
using labs.shared.data.structures;

namespace labs.shared.data.abstracts;

public class Deque<T> : ICollection<T>, IDeque<T>
{
    private Node<T>? head;
    private Node<T>? tail;

    private int size;

    public Deque()
    {
        head = tail = default;
        
        size = 0;
    }

    private void Init(T data)
    {
        if(Count != 0)
            return;
        
        head = new Node<T>(data);
        tail = head;
    }

    public void Back(T data)
    {
        if (Count == 0)
            Init(data);

        else tail = new Node<T>(data, tail);

        size++;
    }

    public void Front(T data)
    {
        if (Count == 0)
            Init(data);

        else head = new Node<T>(data, default, head);

        size++;
    }

    public T Front()
    {
        ThrowableInRange(0, 0, size);

        var data = head!.Data;

        RemoveAt(0);

        return data;
    }

    public T Back()
    {
        ThrowableInRange(0, size - 1, size);

        var data = tail!.Data;
        
        RemoveAt(size - 1);

        return data;
    }

    public void RemoveAt(int index)
    {
        if(Count == 0)
            return;
        
        ThrowableInRange(0, index, size);

        Remove(this[index]);
    }

    protected IEnumerable<T> GetItems()
    {
        if(ReferenceEquals(head, null))
            yield break;
        
        if (ReferenceEquals(head, tail))
        {
            yield return head.Data;
            yield break;
        }
        
        var buffer = head;

        while (!ReferenceEquals(buffer, null) && !ReferenceEquals(head, tail))
        {
            yield return buffer.Data;

            buffer = buffer.Next();
        }
    }

    public void CopyTo(T[] array, int arrayIndex) => Array.Copy(
        this.ToArray(), 0, array, arrayIndex, size
    );

    public bool Remove(T item)
    {
        if (Count == 0)
            return false;

        Node<T>? buffer = head;

        while (buffer != tail)
        {
            if(buffer!.Data!.Equals(item))
                break;

            buffer = buffer.Next();
        }

        if (buffer == null)
            return false;
        
        if(buffer.Prev() != null)
            buffer.Prev()!.Next(buffer.Next());

        if (buffer.Next() != null)
            buffer.Next()!.Prev(buffer.Prev());

        if (Count != 0)
            size--;

        return true;
    }

    public void Add(T item) => Back(item);

    public void Clear()
    {
        if(Count == 0)
            return;

        while (tail != head)
        {
            if (!ReferenceEquals(tail!.Next(), null))
                tail.Next(null);

            tail = tail.Prev();
        }

        tail = head = null;
        size = 0;
    }

    public bool Contains(T item) => this.ToList()
        .Contains(item);

    protected Node<T>? At(int offset)
    {
        ThrowableInRange(0, offset, size);

        Node<T>? buffer = head;
        
        for (int i = 0; i != offset; i++)
            buffer = buffer!.Next();

        return buffer;
    }
    
    public T this[int index]
    {
        get => At(index)!.Data;

        set => At(index)!.Data = value;
    }
    
    public int Count => size;

    public bool IsReadOnly => false;

    private static void ThrowableInRange(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }

    public IEnumerator<T> GetEnumerator() => new DequeEnumerator(head); 

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public class DequeEnumerator : IEnumerator<T>
    {
        private Node<T>? head;
        private Node<T>? current;

        private int index;

        public DequeEnumerator(Node<T>? node)
        {
            head = node;

            index = 0;
            current = null;
        }

        public bool MoveNext()
        {
            if (head == null || index != 0 && current?.Next() == null)
                return false;

            current = (index++ == 0) ? head : current!.Next();
            return true;
        }

        public void Reset()
        {
            index = 0;
            current = default;
        }

        public T Current => current!.Data;

#pragma warning disable CS8603
        object IEnumerator.Current
        {
            get
            {
                if(current == null)
                    throw new ArgumentOutOfRangeException();
                
                return Current;
            }
        }
#pragma warning restore CS8603

        public void Dispose()
        {
        }
    }
}

