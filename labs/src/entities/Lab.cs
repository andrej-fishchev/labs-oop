namespace labs.entities;

public sealed class Lab :
    ILabEntity<int>
{
    public IList<ILabEntity<int>> Tasks
    {
        get; 
        set;
    }

    private readonly ILabEntity<int> entity;

    public Lab(ILabEntity<int>? iface = default, IList<LabTask>? tasks = default)
    {
        entity = iface ?? new IntLabEntity();
        Tasks = (tasks != null)
            ? tasks.Distinct()
                .Select(x => (ILabEntity<int>)x)
                .ToList()
            : new List<ILabEntity<int>>();
    }

    public Lab(int id, string name = "", string description = "") :
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