namespace labs.entities;

public abstract class LabTask :
    ILabEntity<string>
{
    private readonly ILabEntity<string> entity;
    
    // must be initialized from parent sealed class and cannot be changed outside
    
    public IList<ILabEntity<string>> Actions
    {
        get; 
        protected init;
    }

    protected LabTask(string name = "", string description = "") => 
        entity = new StringLabEntity(Guid.NewGuid().ToString(), name, description);

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