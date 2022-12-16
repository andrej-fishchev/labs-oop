using labs.IO;

namespace labs.utils;

public class ArrayRequester<T>
{
    private T[] array;

    public enum FillType
    {
        Randomizer = 0,
        UserInput
    }

    public ArrayRequester(int size = 1)
    {
        array = new T[size];
    }

    public T[] Randomizer(Func<T[], T[]> strategy)
    {
        return strategy.Invoke(array);
    }

    public T[] UserInput(ConsoleDataRequest<T> requester)
    {
        ConsoleDataResponse<T> response;
        for (int i = 0; i < array.Length; i++)
        {
            response = ConsoleIoDataUtils.RequestData(
                requester, 
                $"Введите значение {i + 1}' го элемента: "
            );
            
            if(response.Code == (int) ConsoleDataResponseCode.ConsoleInputRejected)
                break;

            array[i] = response.Data!;
        }
        
        return array;
    }
}