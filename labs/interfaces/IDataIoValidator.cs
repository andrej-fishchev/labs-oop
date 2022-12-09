using labs.interfaces;

namespace labs.IO
{
    public interface IDataIoValidator<T>
    {
        public bool Validate(IDataIoResponse<T> data);
    }
    
    public delegate bool ValidateDataExpression<in T>(T data);    
}

