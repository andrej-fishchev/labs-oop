using System.Text;

namespace labs.lab10.src;

public class Journal :
    Publication
{
    public string Period { get; set; }
    
    public Journal(string name, string dateTime, string period = "1 month") :
        base(name, dateTime)
    {
        Period = period;
    }
    
    public override void Describe(TextWriter target)
    {
        StringBuilder builder = new StringBuilder("Book: \n");

        builder.Append($"Name: {Name} \n");
        builder.Append($"Date: {Date}");
        builder.Append($"Period: {Period}\n");
        
        target.Write(builder.ToString());
    }

    public void NoOverridingDescribe(TextWriter t)
    {
        Describe(t);
    }
}