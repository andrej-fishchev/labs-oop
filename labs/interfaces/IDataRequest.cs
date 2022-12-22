namespace labs.interfaces;

public interface IDataRequest<T>
{
    public IDataResponse<T> Request(IDataValidator<T>? validator = default, bool sendRejectMessage = true);
}