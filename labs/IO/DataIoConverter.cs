using labs.interfaces;

namespace labs.IO;

public class DataIoConverter<TOut> :
    IDataIoResponseConverter<TOut>
{
    public IDataIoResponse<TOut> OuterData { get; set; } 
            
    public DataIoConverter(IDataIoResponse<TOut> emptyObject)
    {
        OuterData = emptyObject;
    }

    public IDataIoResponse<TOut> Convert<T>(IDataIoResponse<T> data, DataConverter<T, TOut> converter)
    {
        OuterData.Code = data.Code;
            
        if (data.Error != null)
            OuterData.Error = data.Error;

        if (data.Data == null)
            return OuterData;

        OuterData.Data = converter.Invoke(data.Data, out var error);
        OuterData.Error = error;

        return OuterData;
    }
}