using labs.IO;

namespace labs.interfaces;

public interface IDataIORequest<T>
{
    public IDataIOResponse<T> Request(DataIoValidator<T> validator);
}