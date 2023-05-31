namespace labs.shared.data.structures;

public class TreeNode<T> : IComparable<T> 
    where T : IComparable<T>
{
    private TreeNode<T>? mHead;
    private TreeNode<T>? mLeft;
    private TreeNode<T>? mRight;

    private readonly T mData;

    public TreeNode(T data, 
        TreeNode<T>? left = null, 
        TreeNode<T>? head = null, 
        TreeNode<T>? right = null
    )
    {
        mHead = head;
        mLeft = left;
        mRight = right;

        mData = data;
    }

    public void Head(TreeNode<T>? value)
    {
        if (mHead != null)
        {
            if (mHead.Left() is { } left && ReferenceEquals(this, left))
                mHead.mLeft = null;

            if (mHead.Right() is { } right && ReferenceEquals(this, right))
                mHead.mRight = null;
        }

        mHead = value;
    }

    public void Left(TreeNode<T>? value)
    {
        if(ReferenceEquals(mLeft, value))
            return;
        
        if (mLeft is { } left)
            left.Head(null);

        mLeft = value;
    }

    public void Right(TreeNode<T>? value)
    {
        if(ReferenceEquals(mRight, value))
            return;
        
        if (mRight is { } right)
            right.Head(null);

        mRight = value;
    }

    public TreeNode<T>? Head() => mHead;
    public TreeNode<T>? Left() => mLeft;
    public TreeNode<T>? Right() => mRight;

    public T Data() => mData;
    
    public int CompareTo(T? other) => mData.CompareTo(other);
}