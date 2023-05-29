namespace labs.shared.utils;

public static class EnumerableItemsPrinter
{
    public static void VerticallyWithNumbers<T>(TextWriter writer, IEnumerator<T> items, string messageOnEmpty)
    {
        int pos = 0;
        while (items.MoveNext())
        {
            writer.WriteLine($"{++pos}. {items.Current} \n");
        }

        if(pos == 0)
            writer.WriteLine(messageOnEmpty);
    }
}