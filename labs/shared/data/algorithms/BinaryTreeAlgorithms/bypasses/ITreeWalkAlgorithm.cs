using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTreeAlgorithms.bypasses;

public interface ITreeWalkAlgorithm<T> : IAlgorithm
{
    public IEnumerable<T> DataWalk(TreeNode<T>? node);

    public IEnumerable<TreeNode<T>> NodeWalk(TreeNode<T>? node);
}