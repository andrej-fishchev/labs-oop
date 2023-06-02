using System.Text;

namespace labs.tasks.lab13.src;

public class EventDequeActionArgs : EventArgs
{
    public string Type { get; set; }
    
    public object? Data { get; set; }

    public EventDequeActionArgs(string type, object? data = null)
    {
        Type = type;
        Data = data;
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append($"On: {Type}").Append('\n')
            .Append($"WithData: {Data ?? '-'}").Append('\n')
            .ToString();
    }
}