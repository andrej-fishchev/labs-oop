using UserDataRequester.responses;
using UserDataRequester.responses.console;

namespace UserDataRequester.requests.console;

public sealed class ConsoleDataRequest :
    IGetRequest,
    IAsyncGetRequest
{
    public TextReader Reader { get; set; }
    
    public TextWriter Writer { get; set; }

    public ConsoleDataRequest(TextReader? reader = default, TextWriter? writer = default)
    {
        Reader = reader ?? Console.In;
        Writer = writer ?? Console.Out;
    }
    
    public async Task<IResponsibleData<object>> AsyncGet(string requestableData)
    {
        await Writer.WriteAsync(requestableData);

        string? buffer = await Reader.ReadLineAsync();

        return new ConsoleResponseData<object>(buffer, buffer == null 
                ? ResponseStatusCodeFactory.Create(ConsoleDataCode.ConsoleTerminated) 
                : default
        );
    }

    public IResponsibleData<object> Get(string requestableData)
    {
        Writer.WriteAsync(requestableData);

        string? buffer = Reader.ReadLine();

        return new ConsoleResponseData<object>(buffer, buffer == null 
            ? ResponseStatusCodeFactory.Create(ConsoleDataCode.ConsoleTerminated)
            : default
        );
    }
}