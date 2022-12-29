using IO.converters;
using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab5;

public sealed class Task1 :
    LabTask
{
    public readonly ConsoleTarget Target = new();
    
    public ConsoleResponseData<int[]> IntArray { get; set; }

    public Task1(string name = "lab5.task1", string description = "") : 
        base(1, name, description)
    {
        IntArray = new ConsoleResponseData<int[]>(new int[10]);
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ручное заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Автоматическое заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Id(3).Name("Удалить все нечетные элементы")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Id(4).Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        SimpleConsoleArrayDataConverter<int> converter =
            BaseTypeArrayDataConverterFactory.MakeIntArrayConverter(delimiter: ";");

        if (type == ArrayGenerationType.UserInput)
        {
            ConsoleResponseData<int[]> buffer = (ConsoleResponseData<int[]>) 
                new ConsoleArrayDataRequest<int>(
                    $"Введите множество целых чисел (через '{converter.Delimiter}'): \n")
                .Request(converter);

            if (buffer.Code == (int)ConsoleResponseDataCode.ConsoleOk)
                IntArray = buffer;
            
            return;
        }

        ConsoleDataRequest<int> request = 
            new ConsoleDataRequest<int>("Введите резмер массива: ");

        ConsoleResponseData<int> size = (ConsoleResponseData<int>)
            request.Request(converter.ElementConverter, new ConsoleDataValidator<int>(
                (data) => data > 0, "значение должно быть больше 0"));

        ConsoleResponseData<int>[] borders = 
            new ConsoleResponseData<int>[2];

        for (int i = 0; i < borders.Length; i++)
        {
            request.DisplayMessage = 
                $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ";

            borders[i] = (ConsoleResponseData<int>)
                request.Request(converter.ElementConverter, new ConsoleDataValidator<int>(
                        (data) =>
                        {
                            if (i == 0)
                                return true;

                            return data > borders[0].Data;
                        }, "значение правой границы должно быть больше левой"),
                    false);
            
            if(borders[i].Code != (int)ConsoleResponseDataCode.ConsoleOk)
                return;
        }

        IntArray.Data = new int[size.Data];
        
        Random random = new Random();

        for (int i = 0; i < IntArray.Data.Length; i++)
            IntArray.Data[i] = random.Next(borders[0].Data, borders[1].Data);
    }

    // удаление нечетных элементов
    public void TaskExpression()
    {
        if(IntArray.Data.Length == 0)
            return;

        IntArray.Data = IntArray.Data
            .Where(x => (x % 2) == 0)
            .ToArray();
    }
    
    public void OutputData()
    {
        for (int i = 0; i < IntArray.Data.Length; i++)
            Target.Write($"{i + 1}: {IntArray.Data[i]} \n");
        
        if(IntArray.Data.Length == 0)
            Target.Write("Массив пуст");
    }
}