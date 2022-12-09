using labs.interfaces;

namespace labs.IO;

public class DataIoValidator<T> :
    IDataIoValidator<T>
{
    public ValidateDataExpression<T> Expression { get; set; }

    public DataIoValidator(ValidateDataExpression<T> ioValidator)
    {
        Expression = ioValidator;
    }

    public bool Validate(IDataIoResponse<T> data)
    {
        return data.Data != null && Expression.Invoke(data.Data);
    }
}