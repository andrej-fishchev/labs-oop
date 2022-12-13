using labs.interfaces;

namespace labs.adapters;

public delegate TK MenuKeyGenerator<T, out TK>(ILabEntity<T> ent, int index);

public static class LabEntityToMenuItemAdapter<T>
{
    public static IDictionary<TK, ILabEntity<T>> Adapt<TK>(
        IList<ILabEntity<T>> entities, 
        MenuKeyGenerator<T, TK> keyGenerator) 
        where TK : notnull
    {
        return entities.Distinct()
            .Select((ent, index) => new {ent, index})
            .ToDictionary(
                (ent) => keyGenerator.Invoke(ent.ent, ent.index), 
                ent => ent.ent);
    }
}