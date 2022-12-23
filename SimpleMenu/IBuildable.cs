namespace SimpleMenu;

public interface IBuildable<out T>
{
    public T Build();
}