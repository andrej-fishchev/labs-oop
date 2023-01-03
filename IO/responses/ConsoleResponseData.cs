namespace IO.responses;

public class ConsoleResponseData<T> :
    IResponsibleData<T>
{
    private ConsoleResponseDataCode code;
 
    private T data;
    
    private string error;
    
    public ConsoleResponseData(T data = default!, string? error = default,
        ConsoleResponseDataCode code = ConsoleResponseDataCode.ConsoleOk)
    {
        this.error = error ?? String.Empty;
        this.data = data;
        this.code = code;
    }

    public string Error() => error;

    public T Data() => data;

    int IResponsibleData<T>.Code() => (int)Code();

    public ConsoleResponseDataCode Code() => code;

    public void Code(ConsoleResponseDataCode value) => code = value;

    public void Error(string value) => error = value;

    public void Data(T value) => data = value;

    public bool IsOk() => 
        !HasError() && Code() == ConsoleResponseDataCode.ConsoleOk;

    public bool HasError() => Error() != String.Empty;

    public static implicit operator bool(ConsoleResponseData<T> obj) 
        => obj.IsOk();

    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, ConsoleResponseDataCode code)
    {
        obj.Code(code);
        return obj;
    }
    
    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, string error)
    {
        obj.Error(error);
        return obj;
    }

    public static bool operator !=(ConsoleResponseData<T> obj, ConsoleResponseDataCode code) 
        => obj.Code() != code;

    public static bool operator ==(ConsoleResponseData<T> obj, ConsoleResponseDataCode code) 
        => obj.Code() == code;

    public object Clone() => ConsoleResponseDataFactory
        .MakeResponse(data, error, code);

    protected bool Equals(ConsoleResponseData<T> other) => 
        Code() == other.Code() 
    && EqualityComparer<T>.Default.Equals(Data(), other.Data()) 
    && Error() == other.Error();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((ConsoleResponseData<T>)obj);
    }

    public override int GetHashCode() => 
        HashCode.Combine((int)Code(), Data(), Error());
}