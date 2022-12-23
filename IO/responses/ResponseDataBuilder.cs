namespace IO.responses;

public abstract class ResponseDataBuilder<T> :
    IBuildable<IResponsibleData<T>>
{
    private readonly IResponsibleData<T> entity;

    protected ResponseDataBuilder(IResponsibleData<T> obj)
    {
        entity = obj;
    }

    public virtual ResponseDataBuilder<T> Data(T value)
    {
        entity.Data = value;
        return this;
    }

    public virtual ResponseDataBuilder<T> Error(string? value)
    {
        entity.Error = value;
        return this;
    }

    public virtual ResponseDataBuilder<T> Code(int value)
    {
        entity.Code = value;
        return this;
    }

    public IResponsibleData<T> Build()
    {
        return entity;
    }
}