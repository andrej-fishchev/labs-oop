namespace labs.entities;

public sealed class LabTaskAction :
    ILabEntity<string>,
    IExecutable
{
    public Action ExecuteAction
    {
        private get; 
        set;
    }

    private ILabEntity<string> entity;

    public LabTaskAction(ILabEntity<string> ent)
    {
        entity = ent;
        ExecuteAction = () => { };
    }

    public LabTaskAction(string name = "", string description = "") :
        this(new StringLabEntity(Guid.NewGuid().ToString(), name, description))
    { }

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
    
    public void Execute() => ExecuteAction.Invoke();
}