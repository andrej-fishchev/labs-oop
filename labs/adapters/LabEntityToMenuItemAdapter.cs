using labs.interfaces;
using labs.menu;

namespace labs.adapters;

public delegate TO MenuKeyGenerator<TV, out TO>(TV ent, int index);

public static class LabEntityToConsoleMenuItemAdapter<T>
{
    public static IMenuItemDictionary<string, T> 
        Adapt(IList<T> entities, MenuKeyGenerator<T, string> keyGenerator)
    {
        return new ConsoleMenuItemDictionary<T>(
            entities.Distinct()
            .Select((ent, index) => new {ent, index})
            .ToDictionary(
                (ent) => keyGenerator.Invoke(ent.ent, ent.index), 
                ent => ent.ent)
        );
    }
}