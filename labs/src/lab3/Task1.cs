using labs.abstracts;
using labs.interfaces;

namespace labs.lab3;

// TODO: task
public sealed class Task1 :
    LabTask
{
    public Task1(
        string name = "lab3.task1", string description = "") : 
        base(1, name, description)
    {
        Actions = new List<ILabEntity<int>>
        {
            Capacity = 0
        };
    }
    
    
}