namespace labs.interfaces;

public interface IDataConverter<TIn, TOut>
{
    public IDataResponse<TOut> Convert(IDataResponse<TIn> data);
}
    