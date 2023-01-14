using IO.validators;

namespace IO.utils;

public static class BaseComparableValidatorFactory
{
    public enum ComparableType
    {
        LessThan = -1,
        Equal,
        GreaterThan
    }
    
    public static ConsoleDataValidator<T> 
        MakeEqualValidator<T>(IComparable<T> obj, ComparableType type, string onErrorMsg = "") 
        where T: IComparable<T> => new(data => data.CompareTo((T?)obj) == (int)type, onErrorMsg);

    public static ConsoleDataValidator<T>
        MakeUnEqualValidator<T>(IComparable<T> obj, ComparableType type, string onErrorMsg = "")
        where T : IComparable<T> => new(data => data.CompareTo((T?)obj) != (int)type, onErrorMsg);

    public static ConsoleDataChainedValidator<T> MakeChainedValidator<T>(IList<IValidatableData<T>> list) => new(list);

    public static ConsoleDataValidator<T> MakeComparableLessThanValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T> => MakeEqualValidator(obj, ComparableType.LessThan, onErrorMsg);

    public static ConsoleDataValidator<T> MakeComparableEqualValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T> => MakeEqualValidator(obj, ComparableType.Equal, onErrorMsg);
    
    
    public static ConsoleDataValidator<T> MakeComparableGreaterThanValidator<T>(IComparable<T> obj, string onErrorMsg) 
        where T : IComparable<T> => MakeEqualValidator(obj, ComparableType.GreaterThan, onErrorMsg);

    // [left; right]
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T> => MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) != less
            // data >= left
            MakeUnEqualValidator(left, ComparableType.LessThan, onErrorMsg),
            
            // data.Compare(right) != greater
            // data <= right
            MakeUnEqualValidator(right, ComparableType.GreaterThan, onErrorMsg)
        });

    // (left; right)
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeStrictValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T> => MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) == greater
            // data > left
            MakeEqualValidator(left, ComparableType.GreaterThan, onErrorMsg),
            
            // data.Compare(left) == less
            // data < right
            MakeEqualValidator(right, ComparableType.LessThan, onErrorMsg)
        });

    // [left; right)
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictLeftValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T> => MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) != less
            // data >= left
            MakeUnEqualValidator(left, ComparableType.LessThan, onErrorMsg),
            
            // data.Compare(left) == less
            // data < right
            MakeEqualValidator(right, ComparableType.LessThan, onErrorMsg)
        });

    // (left; right]
    public static ConsoleDataChainedValidator<T> 
        MakeInRangeNotStrictRightValidator<T>(IComparable<T> left, IComparable<T> right, string onErrorMsg) 
        where T : IComparable<T> => MakeChainedValidator(new List<IValidatableData<T>>
        {
            // data.Compare(left) == greater
            // data > left
            MakeEqualValidator(left, ComparableType.GreaterThan, onErrorMsg),
            
            // data.Compare(right) != greater
            // data <= right
            MakeUnEqualValidator(right, ComparableType.GreaterThan, onErrorMsg)
        });
}