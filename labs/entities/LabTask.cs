using labs.interfaces;

namespace labs.entities;

public abstract class LabTask<T> :
    ILabEntity<T>
{
    public IList<LabTaskAction<T>> Actions
    {
        get;
        set;
    }

    protected LabTask(T id, 
        string name = "unknown", 
        IList<LabTaskAction<T>>? actions = default)
    {
        Id = id;
        Name = name;
        Description = Name;
        Actions = actions ?? new List<LabTaskAction<T>>();
    }

    public T Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}