namespace IO.responses;

public class ConsoleResponseData<T> :
    IResponsibleData<T>
{
    private readonly ConsoleResponseDataCode code;
 
    private readonly T data;
    
    private readonly string error;
    
    public ConsoleResponseData(T data = default!, string? error = default,
        ConsoleResponseDataCode code = ConsoleResponseDataCode.ConsoleOk)
    {
        this.error = error ?? String.Empty;
        this.data = data;
        this.code = code;
    }

    public string Error()
    {
        return error;
    }

    public T Data()
    {
        return data;
    }

    int IResponsibleData<T>.Code()
    {
        return (int)Code();
    }

    public ConsoleResponseDataCode Code()
    {
        return code;
    }

    public bool IsOk()
    {
        return !HasError() && code == ConsoleResponseDataCode.ConsoleOk;
    }

    public bool HasError()
    {
        return Error() != String.Empty;
    }
    
    public static implicit operator bool(ConsoleResponseData<T> obj)
    {
        return obj.IsOk();
    }

    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, ConsoleResponseDataCode code)
    {
        return ConsoleResponseDataFactory.MakeResponse(obj.Data(), obj.Error(), code);
    }
    
    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, string error)
    {
        return ConsoleResponseDataFactory.MakeResponse(obj.Data(), error, obj.Code());
    }
    
    public static ConsoleResponseData<T> operator |(ConsoleResponseData<T> obj, T data)
    {
        return ConsoleResponseDataFactory.MakeResponse(data, obj.Error(), obj.Code());
    }

    public static bool operator !=(ConsoleResponseData<T> obj, ConsoleResponseDataCode code)
    {
        return obj.Code() != code;
    }

    public static bool operator ==(ConsoleResponseData<T> obj, ConsoleResponseDataCode code)
    {
        return obj.Code() == code;
    }

    public object Clone()
    {
        return ConsoleResponseDataFactory.MakeResponse(data, error, code);
    }
    
    protected bool Equals(ConsoleResponseData<T> other)
    {
        return code == other.code 
               && EqualityComparer<T>.Default.Equals(data, other.data) 
               && error == other.error;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((ConsoleResponseData<T>)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)code, data, error);
    }
}