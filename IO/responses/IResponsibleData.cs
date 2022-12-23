namespace IO.responses;

public interface IResponsibleData<T> :
    ICloneable
{
    public string? Error { get; set; }
    
    public int Code { get; set; }

    public T Data { get; set; }
}