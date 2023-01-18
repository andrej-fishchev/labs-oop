namespace labs.lab10.src.comparators;

public class DescribeEqualityComparer<T> :
    IEqualityComparer<T> where T : IDescribe
{
    public bool Equals(T? x, T? y)
    {
        if (x == null || y == null)
            return false;
        
        return GetHashCode(x) == GetHashCode(y);
    }

    public int GetHashCode(T obj)
    {
        return obj.Describe().GetHashCode();
    }
}