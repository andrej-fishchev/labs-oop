using labs.interfaces;

namespace labs.IO;

public class DataIoConverter<TIn, TOut> :
    IDataIoResponseConverter<TIn, TOut>
{
    public IDataIoResponse<TOut> OuterData { get; set; } 
    
    public DataConverter<TIn, TOut> Converter { get; set; }

    public DataIoConverter(DataConverter<TIn, TOut> converter, IDataIoResponse<TOut> emptyObject)
    {
        OuterData = emptyObject;
        Converter = converter;
    }

    public IDataIoResponse<TOut> Convert(IDataIoResponse<TIn> data)
    {
        OuterData.Code = data.Code;
            
        if (data.Error != null)
            OuterData.Error = data.Error;

        if (data.Data == null)
            return OuterData;

        OuterData.Data = Converter.Invoke(data.Data, out var error);
        OuterData.Error = error;

        return OuterData;
    }
}