namespace labs.entities;

public class StringLabEntity :
    ILabEntity<string>
{
    public StringLabEntity(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}