namespace labs.interfaces;

public interface IDataIoValidator<T>
{
    public IDataIoResponse<T> Validate(IDataIoResponse<T> data, string outerText);
}
    
public delegate bool ValidateDataExpression<in T>(T data);