using IO.targets;

namespace labs.entities;

public abstract class LabTask :
    ILabEntity<int>
{
    public static readonly ConsoleTarget Target = new();
    
    private readonly ILabEntity<int> entity;
    
    public IList<ILabEntity<int>> Actions
    {
        get; 
        protected init;
    }
    
    private LabTask(ILabEntity<int>? iface = default, IList<LabTaskAction>? actions = default)
    {
        entity = iface ?? new IntLabEntity();
        Actions = (actions != null)
            ? actions.Distinct()
                .Select(x => (ILabEntity<int>)x)
                .ToList()
            : new List<ILabEntity<int>>();
    }

    protected LabTask(int id, string name = "", string description = "") :
        this(new IntLabEntity(id, name, description))
    { }

    public int Id
    {
        get => entity.Id; 
        set => entity.Id = value;
    }

    public string Name
    {
        get => entity.Name; 
        set => entity.Name = value;
    }

    public string Description
    {
        get => entity.Description; 
        set => entity.Description = value;
    }
}