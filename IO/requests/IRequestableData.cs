using IO.converters;
using IO.responses;
using IO.validators;

namespace IO.requests;

public interface IRequestableData<TIn, TOut>
{
    public IResponsibleData<TOut> Request(
        IConvertibleData<TIn, TOut> converter, 
        IValidatableData<TOut>? validator = default, 
        bool sendRejectMessage = true);
}