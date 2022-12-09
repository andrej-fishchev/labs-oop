using labs.IO;

namespace labs.interfaces;

public interface IDataIoRequest<T>
{
    public IDataIoResponse<T> Request(IDataIoResponseConverter<T> converter);
}