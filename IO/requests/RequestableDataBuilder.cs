using tier1;

namespace IO.requests;

public abstract class RequestableDataBuilder<TIn, TOut> :
    IBuildable<IRequestableData<TIn, TOut>>
{
    private IRequestableData<TIn, TOut> entity;

    public RequestableDataBuilder(IRequestableData<TIn, TOut> value)
    {
        entity = value;
    }

    public IRequestableData<TIn, TOut> Build()
    {
        return entity;
    }
}