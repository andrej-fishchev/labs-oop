namespace IO.targets;

public class ConsoleTarget :
    IDataTarget<TextReader, TextWriter>
{
    public ConsoleTarget(TextReader? input = default, TextWriter? output = default)
    {
        Input = input ?? Console.In;
        Output = output ?? Console.Out;
    }

    public TextReader Input { get; set; }
    
    public TextWriter Output { get; set; }
}