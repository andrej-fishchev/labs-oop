using IO.converters;
using IO.requests;
using IO.responses;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab4;

public sealed class Task1 :
    LabTask
{
    public ConsoleResponseData<int[]> IntArray { get; set; }

    private readonly ConsoleArrayDataRequest<int> 
        arrayRequest = new("");

    private readonly SimpleConsoleDataConverter<int>
        toIntConverter;

    private readonly SimpleConsoleArrayDataConverter<int> 
        arrayConverter;

    public Task1(string name = "lab4.task1", string description = "вариант 24") :
        base(1, name, description)
    {
        toIntConverter = new(int.TryParse);
        
        arrayConverter = new (toIntConverter);
        
        IntArray = new ConsoleResponseData<int[]>(new int[10]);
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ручное заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Автоматическое заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Id(3).Name("Удалить элементы с четными индексами")
                .ExecuteAction(DeleteElements)
                .Build(),

            new LabTaskActionBuilder().Id(4).Name("Добавить элемент с номером K")
                .ExecuteAction(AddElement)
                .Build(),
            
            new LabTaskActionBuilder().Id(5).Name("Циклический сдвиг")
                .ExecuteAction(CyclicShift)
                .Build(),
            
            new LabTaskActionBuilder().Id(6).Name("Найти элемент - среднее арифметическое элементов")
                .ExecuteAction(SearchElement)
                .Build(),
            
            new LabTaskActionBuilder().Id(7).Name("Сортировка вставкой")
                .ExecuteAction(() =>
                {
                    IntArray.Data = InsertSort(IntArray.Data);
                })
                .Build(),
            
            new LabTaskActionBuilder().Id(8).Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build(),
        };
    }

    public void InputData(ArrayGenerationType type)
    {
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                arrayRequest.DisplayMessage =
                    $"Введите множество целых чисел через разделитель ({arrayConverter.Delimiter}): \n";
                
                ConsoleResponseData<int[]> buffer = (ConsoleResponseData<int[]>) 
                    arrayRequest.Request(arrayConverter);

                if (buffer.Code != (int)ConsoleResponseDataCode.ConsoleOk)
                    return;

                IntArray = (ConsoleResponseData<int[]>)
                    buffer.Clone();
                
                break;
            
            case ArrayGenerationType.Randomizer:
                ConsoleResponseData<int> size;
                
                if((size = RequestArraySize()).Code 
                   != (int) ConsoleResponseDataCode.ConsoleOk)
                    return;

                ConsoleResponseData<int>[] borders = 
                    new ConsoleResponseData<int>[2];
                
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = (ConsoleResponseData<int>) 
                        GetIntNumberRequest($"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ")
                        .Request(toIntConverter, ((i == 0)
                            ? null
                            : new ConsoleDataValidator<int>(data => data > borders[0].Data, 
                                "значение правой границы ДСЧ не может быть больше или равно левой")), 
                            false);
                    
                    if(borders[i].Code != (int) ConsoleResponseDataCode.ConsoleOk)
                        return;
                }
                
                IntArray.Data = new int[size.Data];
                
                Random random = new Random();

                for (int i = 0; i < IntArray.Data.Length; i++)
                    IntArray.Data[i] = random.Next(borders[0].Data, borders[1].Data);
                
                break;
        }
    }

    public void DeleteElements()
    {
        IntArray.Data = IntArray.Data
            .Where((_, index) => (index % 2) != 0)
            .ToArray();
    }

    public void AddElement()
    {
        if(IntArray.Data.Length == 0)
            return;
        
        OutputData();

        ConsoleResponseData<int> elementToAdd = (ConsoleResponseData<int>) 
            GetIntNumberRequest("Введите номер элемента: ")
            .Request(toIntConverter,
                new ConsoleDataValidator<int>(data => data > 0 && data <= IntArray.Data.Length, 
            $"значение не может быть меньше 1 и больше {IntArray.Data.Length}"));
        
        if(elementToAdd.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        int[] buffer = new int[IntArray.Data.Length + 1];

        for (int i = 0; i < IntArray.Data.Length; i++)
            buffer[i] = IntArray.Data[i];

        buffer[^1] = buffer[elementToAdd.Data-1];
        
        IntArray.Data = buffer;
        
        arrayRequest.Target.Write(
            $"Элемент с номером {elementToAdd.Data} добавлен в конец массива. \n");
    }

    public void SearchElement()
    {
        if (IntArray.Data.Length == 0)
            return;
        
        int sum = (int) 
            IntArray.Data.Average();

        int result = -1;
        for (int i = 0; i < IntArray.Data.Length; i++)
        {
            if (IntArray.Data[i] == sum)
            {
                result = i;
                break;
            }
        }

        arrayRequest.Target.Write("Элемент - среднее арифметическое суммы элементов массива: " +
            $"{((result == -1) ? "не найден" : $"{IntArray.Data[result].ToString()}")}"
        );
    }

    public int[] InsertSort(int[] array)
    {
        bool f;
        
        for(int i = 1, a, j; i < array.Length; i++)
        {
            a = array[i];
            j = i - 1;
            f = false;
            while(j >= 0 && !f)
            {
                if(!(f = array[j] < a))
                {
                    array[j+1] = array[j];
                    array[j] = a;
                    j--;
                }
            }
        }

        return array;
    }

    public void CyclicShift()
    {
        if(IntArray.Data.Length == 0)
            return;
        
        ConsoleResponseData<int> shiftPower = (ConsoleResponseData<int>) 
            GetIntNumberRequest("Введите силу сдвига [+/- напрвление]: ")
            .Request(toIntConverter);
        
        if(shiftPower.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        shiftPower.Data %= IntArray.Data.Length;
        
        if(shiftPower.Data == 0)
            return;

        IntArray.Data = IntArray.Data
            .Select((x, index) =>
                new { x, nextIndex = ShiftingExpression(index, shiftPower.Data, IntArray.Data.Length) })
            .OrderBy(x => x.nextIndex)
            .Select(x => x.x)
            .ToArray();
    }

    public void OutputData()
    {
        for (int i = 0; i < IntArray.Data.Length; i++)
            arrayRequest.Target.Write($"{i + 1}: {IntArray.Data[i]} \n");
        
        if(IntArray.Data.Length == 0)
            arrayRequest.Target.Write("Массив пуст");
    }

    public int ShiftingExpression(int index, int shiftPower, int length)
    {
        return (length + shiftPower + index) % length;
    }

    public ConsoleResponseData<int> RequestArraySize()
    {
        return (ConsoleResponseData<int>)
            GetIntNumberRequest("Введите размер массива: ")
            .Request(ConsoleDataConverterFactory
                    .MakeSimpleConverter<int>(int.TryParse),
                new ConsoleDataValidator<int>(data => data > 0, 
                    "размер должен быть больше 0"));
    }

    public ConsoleDataRequest<int> GetIntNumberRequest(string message)
    {
        return ConsoleDataRequestFactory.MakeConsoleDataRequest<int>(message);
    }
}