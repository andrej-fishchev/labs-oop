namespace UserDataRequester.responses;

public class ResponseStatusCode
{
    public static readonly ResponseStatusCode ResponseOk = 
        new(0, typeof(ResponseStatusCode));

    private readonly uint code;
    private readonly Type owner;

    public ResponseStatusCode(uint code, Type typeInfo)
    {
        this.code = code;
        owner = typeInfo;
    }

    protected bool Equals(ResponseStatusCode other)
    {
        return code == other.code && owner == other.owner;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(code, owner.GetHashCode());
    }
}