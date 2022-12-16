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

    public IDataIoResponse<T> Validate(IDataIoResponse<T> data, string outerText)
    {
        if (data.Data == null || !Expression.Invoke(data.Data))
            data.Error = outerText;

        return data;
    }
}