using labs.interfaces;

namespace labs.abstracts;

public abstract class DataResponseBuilder<T> :
    IBuildable<IDataResponse<T>>
{
    private readonly IDataResponse<T> entity;

    protected DataResponseBuilder(IDataResponse<T> obj)
    {
        entity = obj;
    }

    public virtual DataResponseBuilder<T> Data(T value)
    {
        entity.Data = value;
        return this;
    }

    public virtual DataResponseBuilder<T> Error(string? value)
    {
        entity.Error = value;
        return this;
    }

    public virtual DataResponseBuilder<T> Code(int value)
    {
        entity.Code = value;
        return this;
    }

    public IDataResponse<T> Build()
    {
        return entity;
    }
}