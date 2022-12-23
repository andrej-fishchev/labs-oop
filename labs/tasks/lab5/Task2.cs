using labs.builders;
using labs.entities;
using labs.factories;
using labs.IO;
using labs.utils;

namespace labs.lab5;

public sealed class Task2 :
    LabTask
{
    public ConsoleDataResponse<int[]>[] IntArray { get; set; }
    
    public readonly ConsoleArrayRequest<int> ArrayRequest = new(
        new ConsoleDataRequest<int>(
            "", new ConsoleDataConverter<int>(DataConverterUtils.ToInt)));

    public Task2(string name = "lab5.task2", string description = "") : 
        base(2, name, description)
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
                
            new LabTaskActionBuilder().Id(3).Name("Добавление строки после строки с наиб. элементом")
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

        switch (type)
        {
            case ArrayGenerationType.UserInput:
                ConsoleDataResponse<int[]>[] buffer = 
                    new ConsoleDataResponse<int[]>[rows.Data];

                for (int i = 0; i < buffer.Length; i++)
                {
                    ArrayRequest.ArraySizeRequest.Target.Write(
                        $"\nЗаполнение строки №{i+1}: \n");
                    
                    buffer[i] = (ConsoleDataResponse<int[]>)
                        ((i == 0)
                            ? ArrayRequest.Request(sendRejectMessage: false)
                            : ((ConsoleArrayRequest<int>)ArrayRequest.Clone()).Request(sendRejectMessage: false));
                    
                    if(buffer[i].Code != (int) ConsoleDataResponseCode.ConsoleOk)
                        return;
                }

                IntArray = buffer;
                break;
            
            case ArrayGenerationType.Randomizer:
                
                ConsoleDataResponse<int> columns = (ConsoleDataResponse<int>)
                    ConsoleDataRequestFactory<int>
                        .MakeConsoleDataRequestFromSelf(ArrayRequest.ArraySizeRequest, 
                            "Введите количество столбцов: ")
                        .Request(ArrayRequest.ArraySizeValidator, false);
                
                if(columns.Code != (int) ConsoleDataResponseCode.ConsoleOk)
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
                        return;
                    
                    borders[i] = randomizerBordersResponse[i].Data;
                }
                
                IntArray = new ConsoleDataResponse<int[]>[rows.Data];

                Random random = new Random();
                
                for (int i = 0; i < IntArray.Length; i++)
                {
                    IntArray[i] = new ConsoleDataResponse<int[]>(new int[columns.Data]);
                    
                    for(int j = 0; j < IntArray[i].Data.Length; j++)
                        IntArray[i].Data[j] = random.Next(borders[0], borders[1]);
                }
                
                break;
        }

    }
    
    public void TaskExpression()
    {
        if(IntArray.Length == 0)
            return;
        
        int row = 0;

        for (int i = 0, max = 0, buf; i < IntArray.Length; i++)
        {
            if (i == 0)
            {
                max = IntArray[i].Data.Max();
                continue;
            }

            if (max < (buf = IntArray[i].Data.Max()))
            {
                max = buf;
                row = i;
            }
        }

        row++;

        ConsoleDataResponse<int[]>[] buffer = 
            new ConsoleDataResponse<int[]>[IntArray.Length + 1];

        ArrayRequest.ArraySize.Data = IntArray[0].Data.Length;

        for (int i = 0, a = 0; i < buffer.Length; i++)
        {
            if (i == row)
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
        
        ArrayRequest.NestedRequest.Target.Write(
            $"\nСтрока добавлена после {row}'й строки\n");
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