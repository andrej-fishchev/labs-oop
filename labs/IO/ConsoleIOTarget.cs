using labs.interfaces;

namespace labs.IO;

public class ConsoleIOTarget :
    IDataIOTarget<TextReader, TextWriter>,
    IReader<string?>,
    IWriter<string>
{
    
    public ConsoleIOTarget(TextReader? input = default, TextWriter? output = default)
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