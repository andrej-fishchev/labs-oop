using labs.delegates;
using labs.interfaces;

namespace labs.IO;

public class ConsoleDataValidator<T> :
    IDataValidator<T>,
    ICloneable
{
    public string ErrorMessage { get; set; }
    
    public DataValidator<T> Validator { get; set; }

    public ConsoleDataValidator(DataValidator<T> ioValidator, string errorMessage = "")
    {
        Validator = ioValidator;
        ErrorMessage = errorMessage;
    }

    public IDataResponse<T> Validate(IDataResponse<T> data)
    {
        if (data is not ConsoleDataResponse<T>)
            throw new InvalidCastException("Ожидался объект типа ConsoleDataResponse");
        
        if (data.Data == null || !Validator.Invoke(data.Data))
            data.Error = ErrorMessage;

        return data;
    }

    public object Clone()
    {
        return new ConsoleDataValidator<T>(Validator, ErrorMessage);
    }
}