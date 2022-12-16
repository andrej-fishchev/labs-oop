namespace labs.interfaces;

public interface IDataIoRequest<T>
{
    public IDataIoResponse<T> Request(IDataIoValidator<T>? validator = default, bool sendRejectMessage = true);
}