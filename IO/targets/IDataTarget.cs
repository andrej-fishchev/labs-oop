namespace IO.targets;

public interface IDataTarget<TI, TO>
    where TI: notnull
    where TO: notnull
{
    public TI Input { get; set; }
    
    public TO Output { get; set; }
}