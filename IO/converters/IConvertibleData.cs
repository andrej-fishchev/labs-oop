using IO.responses;

namespace IO.converters;

public interface IConvertibleData<TIn, TOut>
{
    public IResponsibleData<TOut> Convert(IResponsibleData<TIn> responsibleData);
}
    