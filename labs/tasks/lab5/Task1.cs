using IO.converters;
using IO.requests;
using IO.responses;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab5;

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

    public Task1(string name = "lab5.task1", string description = "") : 
        base(1, name, description)
    {
        toIntConverter = new(int.TryParse);
        
        arrayConverter = new (toIntConverter);
        
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
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                ConsoleResponseData<int[]> buffer = (ConsoleResponseData<int[]>) 
                    arrayRequest.Request(arrayConverter);

                if (buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
                    return;

                IntArray = buffer;
                break;
            
            case ArrayGenerationType.Randomizer:
                ConsoleResponseData<int> size = 
                    InitIntNumberRequest(GetIntNumberRequest("Введите размер массива: "),
                        new ConsoleDataValidator<int>(data => data > 0,
                            "ожидается занчение больше 0"));

                if(size.Code != (int) ConsoleResponseDataCode.ConsoleOk)
                    return;

                ConsoleResponseData<int>[] borders = 
                    new ConsoleResponseData<int>[2];
                
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = InitIntNumberRequest(
                        GetIntNumberRequest($"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: "),
                        ((i == 0)
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
            arrayRequest.Target.Write($"{i + 1}: {IntArray.Data[i]} \n");
        
        if(IntArray.Data.Length == 0)
            arrayRequest.Target.Write("Массив пуст");
    }
    
    public ConsoleResponseData<int> InitIntNumberRequest(ConsoleDataRequest<int> request, 
        ConsoleDataValidator<int>? validator = default, bool send = true)
    {
        return (ConsoleResponseData<int>)
            request.Request(toIntConverter, validator, send);
    }

    public ConsoleDataRequest<int> GetIntNumberRequest(string message)
    {
        return ConsoleDataRequestFactory.MakeConsoleDataRequest<int>(message);
    }
    
}