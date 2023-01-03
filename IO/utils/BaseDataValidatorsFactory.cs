using IO.validators;

namespace IO.utils;

public static class BaseDataValidatorsFactory
{
    public enum ComparableType
    {
        kLessThan = -1,
        kEqual,
        kGreaterThan
    }
    
    public static ConsoleDataValidator<T> 
        MakeComparableValidator<T>(IComparable<T> obj, ComparableType type, string onErrorMsg = "") 
        where T: IComparable<T>
    {
        return new ConsoleDataValidator<T>(data => data.CompareTo((T?)obj) == (int)type,
            onErrorMsg);
    }

    public static ConsoleDataValidator<T>
        MakeComparableInvariantValidator<T>(IComparable<T> obj, ComparableType type, string onErrorMsg = "")
        where T : IComparable<T>
    {
        return new ConsoleDataValidator<T>(data => data.CompareTo((T?)obj) != (int)type,
            onErrorMsg);
    }

    public static ConsoleDataChainedValidator<T> MakeChainedValidator<T>(IList<IValidatableData<T>> list)
    {
        return new ConsoleDataChainedValidator<T>(list);
    }

    public static ConsoleDataValidator<T> MakeComparableLessThanValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeComparableValidator(obj, ComparableType.kLessThan, onErrorMsg);
    }

    public static ConsoleDataValidator<T> MakeComparableEqualValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeComparableValidator(obj, ComparableType.kEqual, onErrorMsg);
    }
    
    public static ConsoleDataValidator<T> MakeComparableGreaterThanValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeComparableValidator(obj, ComparableType.kGreaterThan, onErrorMsg);
    }

    // [left; right]
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) != less
            // data >= left
            MakeComparableInvariantValidator(left, ComparableType.kLessThan, onErrorMsg),
            
            // data.Compare(right) != greater
            // data <= right
            MakeComparableInvariantValidator(right, ComparableType.kGreaterThan, onErrorMsg)
        });
    }

    // (left; right)
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeStrictValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) == greater
            // data > left
            MakeComparableValidator(left, ComparableType.kGreaterThan, onErrorMsg),
            
            // data.Compare(left) == less
            // data < right
            MakeComparableValidator(right, ComparableType.kLessThan, onErrorMsg)
        });
    }
    
    // [left; right)
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictLeftValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) != less
            // data >= left
            MakeComparableInvariantValidator(left, ComparableType.kLessThan, onErrorMsg),
            
            // data.Compare(left) == less
            // data < right
            MakeComparableValidator(right, ComparableType.kLessThan, onErrorMsg)
        });
    }
    
    // (left; right]
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictRightValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T>
    {
        return MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) == greater
            // data > left
            MakeComparableValidator(left, ComparableType.kGreaterThan, onErrorMsg),
            
            // data.Compare(right) != greater
            // data <= right
            MakeComparableInvariantValidator(right, ComparableType.kGreaterThan, onErrorMsg)
        });
    }
}