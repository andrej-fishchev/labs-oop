namespace IO.requests;

public class ConsoleDataRequestBuilder<TOut> :
    RequestableDataBuilder<string?, TOut>
{
    public ConsoleDataRequestBuilder(ConsoleDataRequest<TOut> value) : 
        base(value)
    { }

    public virtual ConsoleDataRequestBuilder<TOut> Display(string value)
    {
        ((ConsoleDataRequest<TOut>)Build())
            .DisplayMessage = value;

        return this;
    }
    
    public virtual ConsoleDataRequestBuilder<TOut> RejectKey(string value)
    {
        ((ConsoleDataRequest<TOut>)Build())
            .RejectKey = value;

        return this;
    }
}