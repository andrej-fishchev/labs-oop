using labs.interfaces;

namespace labs.entities;

public sealed class Lab<T> :
    ILabEntity<T>
{
    public IList<LabTask<T>> Tasks { get; set; }

    public Lab(T id, 
        string name = "unknown", 
        IList<LabTask<T>>? tasks = default)
    {
        Id = id;
        Name = name;
        Description = Name;
        Tasks = tasks ?? new List<LabTask<T>>();
    }

    public T Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}