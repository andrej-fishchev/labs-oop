using labs.interfaces;

namespace labs.entities;

public class IntLabEntity :
    ILabEntity<int>
{
    public IntLabEntity(int id = default, string name = "", string desc = "")
    {
        Id = id;
        Name = name;
        Description = desc;
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}