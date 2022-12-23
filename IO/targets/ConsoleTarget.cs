namespace IO.targets;

public class ConsoleTarget :
    IDataTarget<TextReader, TextWriter>,
    IReader<string?>,
    IWriter<string>
{
    
    public ConsoleTarget(TextReader? input = default, TextWriter? output = default)
    {
        Input = input ?? Console.In;
        Output = output ?? Console.Out;
    }
    
    public string? Read()
    {
        return Input.ReadLine();
    }

    public void Write(string data)
    {
        Output.Write(data);
    }

    public TextReader Input { get; set; }
    
    public TextWriter Output { get; set; }
}