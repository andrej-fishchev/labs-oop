namespace UserDataRequester.responses;


public interface IResponsibleData<T> :
    ICloneable
{
    public T? Data();
    
    public ResponseStatusCode StatusCode();

    public bool IsOk();
    
    public TV As<TV>() where TV : class, IResponsibleData<T> => 
        (this as TV)!;
}