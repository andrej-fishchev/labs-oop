namespace labs.interfaces;

public interface IDataIoResponseConverter<TO>
{
    public IDataIoResponse<TO> Convert<T>(IDataIoResponse<T> data, DataConverter<T, TO> converter);
}
    
public delegate TO DataConverter<in TI, out TO>(TI data, out string? error);