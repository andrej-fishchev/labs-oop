namespace UserDataRequester.validators.console.utils;

public static class BaseComparableValidatorFactory
{
    public enum ComparableType
    {
        LessThan = -1,
        Equal,
        GreaterThan
    }

    public static ConsoleDataValidator MakeComparableTypeEqualValidator(IComparable obj, ComparableType type) => 
        new(data => data is IComparable buffer && buffer.CompareTo(obj) == (int)type);

    public static ConsoleDataValidator MakeComparableTypeUnEqualValidator(IComparable obj, ComparableType type) =>
        new(data => data is IComparable buffer && buffer.CompareTo(obj) != (int)type);

    public static ConsoleDataChainedValidator MakeChainedValidator(IList<IValidatableData> list) => 
        new(list);

    public static ConsoleDataValidator MakeLessThanValidator(IComparable obj) =>
        MakeComparableTypeEqualValidator(obj, ComparableType.LessThan);

    public static ConsoleDataValidator MakeEqualValidator(IComparable obj) =>
        MakeComparableTypeEqualValidator(obj, ComparableType.Equal);
    
    public static ConsoleDataValidator MakeGreaterThanValidator(IComparable obj) =>
        MakeComparableTypeEqualValidator(obj, ComparableType.GreaterThan);

    // [left; right]
    public static ConsoleDataChainedValidator MakeInRangeNotStrictValidator(IComparable left, IComparable right) => 
        MakeChainedValidator(new List<IValidatableData>
        {
            // data.Compare(left) != less
            // data >= left
            MakeComparableTypeUnEqualValidator(left, ComparableType.LessThan),

            // data.Compare(right) != greater
            // data <= right
            MakeComparableTypeUnEqualValidator(right, ComparableType.GreaterThan)
        });

    // (left; right)
    public static ConsoleDataChainedValidator MakeInRangeStrictValidator(IComparable left, IComparable right) =>
        MakeChainedValidator(new List<IValidatableData>
        {
            // data.Compare(left) == greater
            // data > left
            MakeGreaterThanValidator(left),

            // data.Compare(left) == less
            // data < right
            MakeLessThanValidator(right)
        });

    // [left; right)
    public static ConsoleDataChainedValidator MakeInRangeNotStrictLeftValidator(IComparable left, IComparable right) =>
        MakeChainedValidator(new List<IValidatableData>
        {
            // data.Compare(left) != less
            // data >= left
            MakeComparableTypeUnEqualValidator(left, ComparableType.LessThan),

            // data.Compare(left) == less
            // data < right
            MakeLessThanValidator(right)
        });

    // (left; right]
    public static ConsoleDataChainedValidator MakeInRangeNotStrictRightValidator(IComparable left, IComparable right) =>
        MakeChainedValidator(new List<IValidatableData>
        {
            // data.Compare(left) == greater
            // data > left
            MakeGreaterThanValidator(left),

            // data.Compare(right) != greater
            // data <= right
            MakeComparableTypeUnEqualValidator(right, ComparableType.GreaterThan)
        });
}