using labs.interfaces;

namespace labs.entities;

public sealed class LabTaskAction<T> :
    ILabEntity<T>,
    IAction
{
    public Action Delegator
    {
        private get;
        set;
    }

    public LabTaskAction(T id, string name = "", Action? action = null)
    {
        Id = id;
        Name = name;
        Description = "";
        
        Delegator = action ?? (() => { });
    }

    public void Invoke()
    {
        Delegator.Invoke();
    }

    public T Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}