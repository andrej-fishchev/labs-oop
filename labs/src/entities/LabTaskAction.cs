namespace labs.entities;

public sealed class LabTaskAction :
    ILabEntity<int>,
    IExecutable
{
    public Action ExecuteAction
    {
        private get; 
        set;
    }

    private ILabEntity<int> entity;

    public LabTaskAction(ILabEntity<int>? iface = default, Action? excute = default)
    {
        entity = iface ?? new IntLabEntity();
        ExecuteAction = excute ?? (() => {});
    }

    public LabTaskAction(int id, string name = "", string description = "") :
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
    
    public void Execute()
    {
        ExecuteAction.Invoke();
    }
}