using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTree.walks;

public class PostOrderTreeWalk<T> : ITreeWalkAlgorithm<T> 
    where T : IComparable<T>
{
    public IEnumerable<T> DataWalk(TreeNode<T>? node)
    {
        if (node == null)
            yield break;
        
        foreach (var left in DataWalk(node.Left()))
            yield return left;
        
        foreach (var right in DataWalk(node.Right()))
            yield return right;
        
        yield return node.Data();
    }

    public IEnumerable<TreeNode<T>> NodeWalk(TreeNode<T>? node)
    {
        if (node == null)
            yield break;

        foreach (var left in NodeWalk(node.Left()))
            yield return left;
        
        foreach (var right in NodeWalk(node.Right()))
            yield return right;
        
        yield return node;
    }
}