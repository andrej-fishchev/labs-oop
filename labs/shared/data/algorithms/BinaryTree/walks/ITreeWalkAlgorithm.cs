using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTree.walks;

public interface ITreeWalkAlgorithm<T> : IAlgorithm 
    where T : IComparable<T>
{
    public IEnumerable<T> DataWalk(TreeNode<T>? node);

    public IEnumerable<TreeNode<T>> NodeWalk(TreeNode<T>? node);
}