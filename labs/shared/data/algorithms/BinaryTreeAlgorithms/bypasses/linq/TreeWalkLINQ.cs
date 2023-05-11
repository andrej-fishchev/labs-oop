using labs.shared.data.structures;

namespace labs.shared.data.algorithms.BinaryTreeAlgorithms.bypasses.linq;

public static class TreeWalkLINQ
{
    public static IEnumerable<T> DataWalk<T>(this TreeNode<T>? node, ITreeWalkAlgorithm<T> algorithm) => 
        algorithm.DataWalk(node);
    
    public static IEnumerable<TreeNode<T>> NodeWalk<T>(this TreeNode<T>? node, ITreeWalkAlgorithm<T> algorithm) => 
        algorithm.NodeWalk(node);
}