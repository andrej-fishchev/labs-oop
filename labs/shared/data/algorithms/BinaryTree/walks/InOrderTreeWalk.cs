using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTree.walks;

public class InOrderTreeWalk<T> : ITreeWalkAlgorithm<T> 
    where T : IComparable<T>
{
    public IEnumerable<T> DataWalk(TreeNode<T>? node)
    {
        if (node == null)
            yield break;

        foreach (var left in DataWalk(node.Left()))
            yield return left;
        
        yield return node.Data();

        foreach (var right in DataWalk(node.Right()))
            yield return right;
    }

    public IEnumerable<TreeNode<T>> NodeWalk(TreeNode<T>? node)
    {
        if (node == null)
            yield break;

        foreach (var left in NodeWalk(node.Left()))
            yield return left;
        
        yield return node;

        foreach (var right in NodeWalk(node.Right()))
            yield return right;
    }
}