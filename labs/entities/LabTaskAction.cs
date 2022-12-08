using labs.interfaces;

namespace labs.entities;

public sealed class LabTaskAction<T> :
    ILabEntity<T>,
    IExecutable
{
    public Action ExecuteAction
    {
        private get;
        set;
    }

    public LabTaskAction(T id, string name = "", Action? action = null)
    {
        Id = id;
        Name = name;
        Description = "";
        
        ExecuteAction = action ?? (() => { });
    }

    public void Execute()
    {
        ExecuteAction.Invoke();
    }

    public T Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}