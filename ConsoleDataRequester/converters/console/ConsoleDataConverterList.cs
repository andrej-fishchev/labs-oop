using System.Collections;
using UserDataRequester.responses;
using UserDataRequester.responses.console;

namespace UserDataRequester.converters.console;

public class ConsoleDataConverterList :
    IList<IConvertibleData>,
    IConvertibleData
{
    private readonly IList<IConvertibleData> converters;

    public ConsoleDataConverterList(IList<IConvertibleData> list) =>
        converters = list;

    public IResponsibleData<object> Convert(IResponsibleData<object> responsibleData)
    {
        if (!responsibleData.IsOk())
            return ConsoleResponseDataFactory.MakeResponse<object>(code: responsibleData.StatusCode());

        ConsoleResponseData<object> buffer;
        foreach (var t in converters)
            if ((buffer = t.Convert(responsibleData).As<ConsoleResponseData<object>>()).IsOk())
                return buffer;

        return ConsoleResponseDataFactory.MakeResponse<object>(
            code: ResponseStatusCodeFactory.Create(ConsoleDataConverterCode.UnableToConvertData)
        );
    }

    public IEnumerator<IConvertibleData> GetEnumerator()
    {
        return converters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return converters.GetEnumerator();
    }

    public void Add(IConvertibleData item)
    {
        converters.Add(item);
    }

    public void Clear()
    {
        converters.Clear();
    }

    public bool Contains(IConvertibleData item)
    {
        return converters.Contains(item);
    }

    public void CopyTo(IConvertibleData[] array, int arrayIndex)
    {
        converters.CopyTo(array, arrayIndex);
    }

    public bool Remove(IConvertibleData item)
    {
        return converters.Remove(item);
    }

    public int Count => converters.Count;

    public bool IsReadOnly => converters.IsReadOnly;

    public int IndexOf(IConvertibleData item)
    {
        return converters.IndexOf(item);
    }

    public void Insert(int index, IConvertibleData item)
    {
        converters.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        converters.RemoveAt(index);
    }

    public IConvertibleData this[int index]
    {
        get => converters[index];
        set => converters[index] = value;
    }
}