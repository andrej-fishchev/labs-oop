using labs.shared.data.abstracts;

namespace labs.shared.data.algorithms.CustomHashTable.linq;

public static class CustomHashTableLinq
{
    public static IEnumerable<T>? ChooseContainer<T>(this CustomHashTable<T> source, Predicate<T> condition)
    {
        return source.InnerContainer()
            .Where(chain => chain != null)
            .FirstOrDefault(chain => chain.FirstOrDefault(condition.Invoke) != null);
    }
}