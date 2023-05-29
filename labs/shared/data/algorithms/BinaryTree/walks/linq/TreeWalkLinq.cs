using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTree.walks.linq;

public static class TreeWalkLinq
{
    public static IEnumerable<T> DataWalk<T>(this TreeNode<T>? node, ITreeWalkAlgorithm<T> algorithm) 
        where T : IComparable<T> => algorithm.DataWalk(node);
    
    public static IEnumerable<TreeNode<T>> NodeWalk<T>(this TreeNode<T>? node, ITreeWalkAlgorithm<T> algorithm) 
        where T : IComparable<T> => algorithm.NodeWalk(node);
}