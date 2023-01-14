namespace tier1;

public interface IBuildable<out T>
{
    public T Build();
}