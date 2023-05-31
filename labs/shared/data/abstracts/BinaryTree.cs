using System.Collections;
using labs.shared.data.algorithms.BinaryTree.walks;
using labs.shared.data.algorithms.BinaryTree.walks.linq;
using labs.shared.data.structures;

namespace labs.shared.data.abstracts;

public sealed class BinaryTree<T> : IEnumerable<T> 
    where T : IComparable<T>
{
    public ITreeWalkAlgorithm<T> TreeWalkAlgorithm { get; set; }
    
    private TreeNode<T>? mHead;
    private TreeNode<T>? mCurrent;

    public BinaryTree(ITreeWalkAlgorithm<T> treeWalkAlgorithm)
    {
        mHead = default;
        mCurrent = default;

        TreeWalkAlgorithm = treeWalkAlgorithm;
    }
    
    public bool Add(T value, ITreeWalkAlgorithm<T>? algorithm = default) =>
        Add(new TreeNode<T>(value), algorithm);

    public bool Add(TreeNode<T>? value, ITreeWalkAlgorithm<T>? algorithm = default)
    {
        if(value == null)
            return false;
        
        if (mHead == null)
        {
            mHead = value;
            return true;
        }

        mCurrent = mHead.NodeWalk(algorithm ?? TreeWalkAlgorithm)
            .FirstOrDefault(x => x.CompareTo(value.Data()) == 0);

        if (mCurrent != null)
            return false;
        
        mCurrent = mCurrent.NodeWalk(algorithm ?? TreeWalkAlgorithm)
            .First(x => x.Left() == null || x.Right() == null);
        
        Action<TreeNode<T>> func = (mCurrent.Left() == null) 
            ? mCurrent.Left 
            : mCurrent.Right;

        value.Head(mCurrent);
        
        func.Invoke(value);
        return true;
    }

    public void Remove(T value, ITreeWalkAlgorithm<T>? algorithm = default)
    {
        if(mHead == null)
            return;

        mCurrent ??= mHead;

        try
        {
            mCurrent = mCurrent.NodeWalk(algorithm ?? TreeWalkAlgorithm)
                .First(x => x.CompareTo(value) == 0);
        }
        catch (InvalidOperationException)
        {
            return;
        }

        TreeNode<T>? mainBranch = mCurrent.Left() != null
            ? mCurrent.Left()
            : mCurrent.Right() != null
                ? mCurrent.Right()
                : null;

        mainBranch?.Head(mCurrent.Head());

        if (mHead == mCurrent)
            mHead = mainBranch;

        if (mainBranch != null)
            Add(mCurrent.Left() == mainBranch ? mCurrent.Right() : mCurrent.Left());
    }

    public void Clear()
    {
        mHead = default;
        mCurrent = default;
    }

    public TreeNode<T>? InternalNode() => mHead;

    public IEnumerator<T> GetEnumerator() => 
        InternalNode().DataWalk(TreeWalkAlgorithm).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}