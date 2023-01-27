namespace labs.entities;

public sealed class Lab :
    ILabEntity<string>
{
    public IList<ILabEntity<string>> Tasks
    {
        get;
        set;
    }

    private readonly ILabEntity<string> entity;
    
    public Lab(string name = "", string description = "") :
        this(new StringLabEntity(Guid.NewGuid().ToString(), name, description))
    { }
    
    private Lab(ILabEntity<string> ent)
    {
        entity = ent;
        Tasks = new List<ILabEntity<string>>();
    }
    
    public string Id
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