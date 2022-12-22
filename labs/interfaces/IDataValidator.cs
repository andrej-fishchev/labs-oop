namespace labs.interfaces;

public interface IDataValidator<T>
{
    public IDataResponse<T> Validate(IDataResponse<T> data);
}
    