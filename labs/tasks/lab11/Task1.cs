using labs.entities;
using labs.lab10.src;
using labs.lab9.src;

namespace labs.lab11;

public sealed class Task1 : LabTask
{
    public List_t<Publication> Publications
    {
        get; 
        private set;
    }

    public Task1(string name = "lab11.task1", string description = "") :
        base(1, name, description)
    {
        Actions = new List<ILabEntity<int>>();
    }
    
        
}