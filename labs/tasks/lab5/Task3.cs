using labs.builders;
using labs.entities;
using labs.factories;
using labs.IO;
using labs.utils;

namespace labs.lab5;

public sealed class Task3 :
    LabTask
{
    public ConsoleDataResponse<int[]>[] IntArray { get; set; }
    
    public readonly ConsoleArrayRequest<int> ArrayRequest = new(
        new ConsoleDataRequest<int>(
            "", new ConsoleDataConverter<int>(DataConverterUtils.ToInt)));

    public Task3(string name = "lab5.task3", string description = "") : 
        base(3, name, description)
    {
        IntArray = new ConsoleDataResponse<int[]>[]
        {
            new(new int[1])
        };

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ручное заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Автоматическое заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Id(3).Name("Добавить строку в начало массива")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Id(4).Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        ArrayRequest.ArraySize.Data = 0;
        
        ConsoleDataResponse<int> rows = (ConsoleDataResponse<int>)
            ConsoleDataRequestFactory<int>
                .MakeConsoleDataRequestFromSelf(ArrayRequest.ArraySizeRequest, 
                    "Введите количество строк: ")
                .Request(ArrayRequest.ArraySizeValidator);

        if(rows.Code != (int) ConsoleDataResponseCode.ConsoleOk)
            return;

        ConsoleDataResponse<int[]>[] buffer = 
            new ConsoleDataResponse<int[]>[rows.Data];
        
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                
                for (int i = 0; i < buffer.Length; i++)
                {
                    ArrayRequest.ArraySize.Data = 0;

                    buffer[i] = (ConsoleDataResponse<int[]>)
                        ArrayRequest.Request(sendRejectMessage: false);

                    if(buffer[i].Code != (int) ConsoleDataResponseCode.ConsoleOk)
                        return;
                }
                
                break;
            
            case ArrayGenerationType.Randomizer:

                ConsoleDataRequest<int> bufferedRequest = ConsoleDataRequestFactory<int>
                        .MakeConsoleDataRequestFromSelf(ArrayRequest.ArraySizeRequest);

                ConsoleDataResponse<int> bufferedSize;

                for (int i = 0; i < rows.Data; i++)
                {
                    bufferedRequest.DisplayMessage = $"Введите размер для {i + 1}'й строки: ";
                    
                    if((bufferedSize = (ConsoleDataResponse<int>)
                           bufferedRequest
                               .Request(ArrayRequest.ArraySizeValidator, sendRejectMessage: false)).Code
                       != (int) ConsoleDataResponseCode.ConsoleOk)
                        return;

                    buffer[i] = new ConsoleDataResponse<int[]>(new int[bufferedSize.Data]);
                }
                
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
                        return;
                    
                    borders[i] = randomizerBordersResponse[i].Data;
                }
                
                Random random = new Random();
                
                for (int i = 0; i < buffer.Length; i++)
                    for(int j = 0; j < buffer[i].Data.Length; j++)
                        buffer[i].Data[j] = random.Next(borders[0], borders[1]);
                
                break;
        }

        IntArray = buffer;
    }
    
    public void TaskExpression()
    {
        if(IntArray.Length == 0)
            return;

        ConsoleDataResponse<int[]>[] buffer = 
            new ConsoleDataResponse<int[]>[IntArray.Length + 1];

        ArrayRequest.ArraySize.Data = 0;

        for (int i = 0, a = 0; i < buffer.Length; i++)
        {
            if (i == 0)
            {
                ArrayRequest.NestedRequest.Target.Write(
                    "\nДобавление строки: \n");        
                
                buffer[i] = (ConsoleDataResponse<int[]>)
                    ArrayRequest.Request();
                
                if(buffer[i].Code != (int) ConsoleDataResponseCode.ConsoleOk)
                    return;
                
                continue;
            }

            buffer[i] = IntArray[a++];
        }

        IntArray = buffer;
        
        ArrayRequest.NestedRequest.Target.Write("\nСтрока добавлена\n");
    }

    public void OutputData()
    {
        for (int i = 0; i < IntArray.Length; i++)
        {
            for (int j = 0; j < IntArray[i].Data.Length; j++)
            {
                ArrayRequest.NestedRequest.Target.Write(
                    $"{IntArray[i].Data[j]}");
                
                if(j + 1 != IntArray[i].Data.Length)
                    ArrayRequest.NestedRequest.Target.Write("\t");
            }
            
            ArrayRequest.NestedRequest.Target.Write("\n");
        }
        
        if(IntArray.Length == 0)
            ArrayRequest.NestedRequest.Target.Write("Массив пуст");
    }
}