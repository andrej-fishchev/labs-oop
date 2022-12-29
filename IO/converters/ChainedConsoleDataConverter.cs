using System.Collections;
using IO.responses;

namespace IO.converters;

public class ChainedConsoleDataConverter<TOut> :
    IList<IConvertibleData<string?, TOut>>,
    IConvertibleData<string?, TOut>
{
    private readonly IList<IConvertibleData<string?, TOut>> list;
    
    public ChainedConsoleDataConverter(IList<IConvertibleData<string?, TOut>>? list = default)
    {
        this.list = list ?? new List<IConvertibleData<string?, TOut>>();
    }

    public IResponsibleData<TOut> Convert(IResponsibleData<string?> responsibleData)
    {
        IResponsibleData<TOut> output = new ConsoleResponseData<TOut>(
            code: responsibleData.Code, error: responsibleData.Error);

        for (int i = 0; i < list.Count; i++)
        {
            output = list[i].Convert(responsibleData);

            if (output.Code == (int)ConsoleResponseDataCode.ConsoleOk)
                return output;
        }

        output.Error = $"не удалось выполнить преобразование для '{responsibleData.Data}'";
        output.Code = (int)ConsoleResponseDataCode.ConsoleInvalidData;

        return output;
    }

    public IEnumerator<IConvertibleData<string?, TOut>> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return list.GetEnumerator();
    }

    public void Add(IConvertibleData<string?, TOut> item)
    {
        list.Add(item);
    }

    public void Clear()
    {
        list.Clear();
    }

    public bool Contains(IConvertibleData<string?, TOut> item)
    {
        return list.Contains(item);
    }

    public void CopyTo(IConvertibleData<string?, TOut>[] array, int arrayIndex)
    {
        list.CopyTo(array, arrayIndex);
    }

    public bool Remove(IConvertibleData<string?, TOut> item)
    {
        return list.Remove(item);
    }

    public int Count => list.Count;
    
    public bool IsReadOnly => list.IsReadOnly;

    public int IndexOf(IConvertibleData<string?, TOut> item)
    {
        return list.IndexOf(item);
    }

    public void Insert(int index, IConvertibleData<string?, TOut> item)
    {
        list.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
    }

    public IConvertibleData<string?, TOut> this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }
}