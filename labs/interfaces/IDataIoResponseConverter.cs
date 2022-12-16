namespace labs.interfaces;

public interface IDataIoResponseConverter<TIn, TOut>
{
    public IDataIoResponse<TOut> Convert(IDataIoResponse<TIn> data);
}
    
public delegate TO DataConverter<in TI, out TO>(TI data, out string? error);