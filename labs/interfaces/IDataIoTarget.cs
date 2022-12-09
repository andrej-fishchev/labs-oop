namespace labs.interfaces;

public interface IDataIoTarget<TI, TO>
    where TI: notnull
    where TO: notnull
{
    public TI Input { get; set; }
    
    public TO Output { get; set; }
}