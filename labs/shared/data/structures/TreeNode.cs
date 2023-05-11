namespace labs.shared.data.structures;

public class TreeNode<T>
{
    private TreeNode<T>? m_Head;
    private TreeNode<T>? m_Left;
    private TreeNode<T>? m_Right;

    private T m_Data;

    public TreeNode(T data, 
        TreeNode<T>? left = null, 
        TreeNode<T>? head = null, 
        TreeNode<T>? right = null
    )
    {
        m_Head = head;
        m_Left = left;
        m_Right = right;

        m_Data = data;
    }

    public void Head(TreeNode<T>? value)
    {
        if (m_Head != null)
        {
            if (m_Head.Left() is { } left && left == this)
                m_Head.Left(null);

            if (m_Head.Right() is { } right && right == this)
                m_Head.Right(null);
        }

        m_Head = value;
    }

    public void Left(TreeNode<T>? value)
    {
        if(ReferenceEquals(m_Left, value))
            return;
        
        if (m_Left is { } left)
            left.Head(null);

        m_Left = value;
    }

    public void Right(TreeNode<T>? value)
    {
        if(ReferenceEquals(m_Right, value))
            return;
        
        if (m_Right is { } right)
            right.Head(null);

        m_Right = value;
    }

    public TreeNode<T>? Head() => m_Head;
    public TreeNode<T>? Left() => m_Left;
    public TreeNode<T>? Right() => m_Right;

    public T Data() => m_Data;
}