namespace labs.interfaces;

public interface IBuildable<out T>
{
    public T Build();
}