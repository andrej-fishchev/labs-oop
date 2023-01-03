namespace IO.responses;

public interface IResponsibleData<T> :
    ICloneable
{
    public string Error();

    public T Data();

    public int Code();

    public bool IsOk();

    public bool HasError();

    public virtual TV As<TV>() where TV : class, IResponsibleData<T> 
        => (this as TV)!;
}