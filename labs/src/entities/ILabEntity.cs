namespace labs.entities;

public interface ILabEntity<T>
{
    public T Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}