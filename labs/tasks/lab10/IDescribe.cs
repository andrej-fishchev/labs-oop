namespace labs.lab10;

public interface IDescribe :
    IComparer<IDescribe>,
    IComparable<IDescribe>
{
    public string Describe();
    
    int IComparer<IDescribe>.Compare(IDescribe? x, IDescribe? y)
    {
        if (x == null) return -1;
        if (y == null) return 1;
        
        return string.CompareOrdinal(x.Describe(), y.Describe());
    }

    int IComparable<IDescribe>.CompareTo(IDescribe? other)
    {
        return Compare(this, other);
    }
}