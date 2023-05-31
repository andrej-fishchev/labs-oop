namespace UserDataRequester.responses;

public static class ResponseStatusCodeFactory
{
    public static ResponseStatusCode Create<T>(T code) where T: Enum => 
        new(Convert.ToUInt32(code), typeof(T));
}