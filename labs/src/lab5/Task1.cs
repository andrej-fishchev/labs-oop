using labs.abstracts;
using labs.builders;
using labs.factories;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab5;

public sealed class Task1 :
    LabTask
{
    public ConsoleDataResponse<int[]> IntArray { get; set; }
    
    public readonly ConsoleArrayRequest<int> ArrayRequest = new(
        new ConsoleDataRequest<int>(
            "", new ConsoleDataConverter<int>(DataConverterUtils.ToInt)));

    public Task1(string name = "lab5.task1", string description = "") : 
        base(1, name, description)
    {
        IntArray = new ConsoleDataResponse<int[]>(new int[10]);
        
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
                ConsoleDataResponse<int[]> buffer = (ConsoleDataResponse<int[]>) 
                    ArrayRequest.Request();

                if (buffer.Code != (int)ConsoleDataResponseCode.ConsoleOk)
                    return;

                IntArray = (ConsoleDataResponse<int[]>)
                    buffer.Clone();
                
                break;
            
            case ArrayGenerationType.Randomizer:
                ConsoleDataResponse<int> size = (ConsoleDataResponse<int>)
                    ArrayRequest.ArraySizeRequest.Request(ArrayRequest.ArraySizeValidator);
                
                if(size.Code != (int) ConsoleDataResponseCode.ConsoleOk)
                    return;

                int[] borders = new int[2];
                
                ConsoleDataResponse<int>[] randomizerBordersResponse = 
                    new ConsoleDataResponse<int>[borders.Length];
                
                for (int i = 0; i < borders.Length; i++)
                {
                    randomizerBordersResponse[i] = (ConsoleDataResponse<int>) 
                        ConsoleDataRequestFactory<int>
                        .MakeConsoleDataRequestFromSelf(ArrayRequest.ArraySizeRequest, 
                            $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ")
                        .Request(((i == 0)
                            ? null
                            : new ConsoleDataValidator<int>(data => data > borders[0], 
                                "значение правой границы ДСЧ не может быть больше или равно левой")), 
                            false);
                    
                    if(randomizerBordersResponse[i].Code != (int) ConsoleDataResponseCode.ConsoleOk)
                        break;
                    
                    borders[i] = randomizerBordersResponse[i].Data;
                }
                
                IntArray.Data = new int[size.Data];
                
                Random random = new Random();

                for (int i = 0; i < IntArray.Data.Length; i++)
                    IntArray.Data[i] = random.Next(borders[0], borders[1]);
                
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
            ArrayRequest.ArraySizeRequest.Target.Write($"{i + 1}: {IntArray.Data[i]} \n");
        
        if(IntArray.Data.Length == 0)
            ArrayRequest.ArraySizeRequest.Target.Write("Массив пуст");
    }
}