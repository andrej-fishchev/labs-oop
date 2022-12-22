using labs.abstracts;
using labs.builders;
using labs.factories;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab6;

public sealed class Task1 :
    LabTask
{
    public ConsoleDataResponse<double[]>[] IntArray { get; set; }
    
    public readonly ConsoleArrayRequest<double> ArrayRequest = new(
        new ConsoleDataRequest<double>(
            "", new ConsoleDataConverter<double>(
                DataConverterUtils.ToDoubleWithInvariant)));

    public Task1(string name = "lab6.task1", string description = "") : 
        base(1, name, description)
    {
        IntArray = new ConsoleDataResponse<double[]>[]
        {
            new(new double[1])
        };

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ручное заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Автоматическое заполнение массива")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Id(3).Name("Сортировка элементов и строк по возрастанию")
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

        ConsoleDataResponse<double[]>[] buffer = 
            new ConsoleDataResponse<double[]>[rows.Data];
        
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                
                for (int i = 0; i < buffer.Length; i++)
                {
                    ArrayRequest.ArraySize.Data = 0;

                    buffer[i] = (ConsoleDataResponse<double[]>)
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

                    buffer[i] = new ConsoleDataResponse<double[]>(new double[bufferedSize.Data]);
                }
                
                ConsoleDataResponse<int>[] borders = 
                    new ConsoleDataResponse<int>[2];
                
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = (ConsoleDataResponse<int>) 
                        ConsoleDataRequestFactory<int>
                        .MakeConsoleDataRequestFromSelf(ArrayRequest.ArraySizeRequest, 
                            $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ")
                        .Request(((i == 0)
                            ? null
                            : new ConsoleDataValidator<int>(data => data > borders[0].Data, 
                                "значение правой границы ДСЧ не может быть больше или равно левой")), 
                            false);
                    
                    if(borders[i].Code != (int) ConsoleDataResponseCode.ConsoleOk)
                        return;
                }
                
                Random random = new Random();
                
                for (int i = 0; i < buffer.Length; i++)
                    for(int j = 0; j < buffer[i].Data.Length; j++)
                        buffer[i].Data[j] = random.Next(borders[0].Data, borders[1].Data);
                
                break;
        }

        IntArray = buffer;
    }
    
    public void TaskExpression()
    {
        if(IntArray.Length == 0)
            return;

        IntArray = IntArray.OrderBy(x => x.Data.Length)
            .ToArray();

        for (int i = 0; i < IntArray.Length; i++)
            Array.Sort(IntArray[i].Data);
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