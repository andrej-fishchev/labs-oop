namespace UserDataRequester.responses.console;

public class ConsoleResponseData<T> :
    IResponsibleData<T>
{
    private ResponseStatusCode code;

    private T? data;

    public ConsoleResponseData(T? data = default, ResponseStatusCode? code = default)
    {
        this.data = data;
        this.code = code ?? ResponseStatusCode.ResponseOk;
    }

    public T? Data() => data;
    
    public void Date(T? value) => data = value;
    
    public ResponseStatusCode StatusCode() => code;
    
    public void StatusCode(ResponseStatusCode value) => code = value;
    
    public bool IsOk() => StatusCode() == ResponseStatusCode.ResponseOk;
    
    public object Clone() => new ConsoleResponseData<T>(data, code);

    public static implicit operator bool(ConsoleResponseData<T> obj) => 
        obj.IsOk();

    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, ResponseStatusCode code)
    {
        obj.StatusCode(code);
        return obj;
    }

    public static bool operator !=(ConsoleResponseData<T> obj, ResponseStatusCode code)
    {
        return obj.StatusCode() != code;
    }

    public static bool operator ==(ConsoleResponseData<T> obj, ResponseStatusCode code)
    {
        return obj.StatusCode() == code;
    }

    private bool Equals(IResponsibleData<T> other)
    {
        return StatusCode() == other.StatusCode()
               && EqualityComparer<T>.Default.Equals(Data(), other.Data());
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((ConsoleResponseData<T>)obj);
    }

    public override int GetHashCode() => HashCode.Combine(StatusCode(), Data());
}