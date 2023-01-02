using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab5;

public sealed class Task3 : LabTask
{
    public ConsoleResponseData<int[]>[] IntArray 
    { 
        get; 
        private set; 
    }

    public Task3(string name = "lab5.task3", string description = "") : 
        base(3, name, description)
    {
        IntArray = new ConsoleResponseData<int[]>[]
        {
            new(Array.Empty<int>())
        };

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Создать рваный массив")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Name("Создать рваный массив [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Name("Добавить строку в начало массива")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Name("Вывод рваного массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        ConsoleDataValidator<int> sizeValidator = new ConsoleDataValidator<int>(
            data => data > 0,
            "ожидалось значение больше 0"
        );

        ConsoleResponseData<int> rows = new ConsoleDataRequest<int>("Введите количество массивов: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), sizeValidator)
            .As<ConsoleResponseData<int>>();

        if(!rows) return;

        ConsoleResponseData<int[]>[] buffer = 
            new ConsoleResponseData<int[]>[rows.Data()];
        
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = new ConsoleDataRequest<int[]>($"Введите {i + 1}'е множество целых чисел: \n")
                        .Request(BaseTypeArrayDataConverterFactory.MakeIntArrayConverter(), sendRejectMessage: false)
                        .As<ConsoleResponseData<int[]>>();

                    if(!buffer[i]) return;
                }
                
                break;
            
            case ArrayGenerationType.Randomizer:

                ConsoleDataRequest<int> request = 
                    new ConsoleDataRequest<int>("");

                ConsoleResponseData<int> bufferedSize;

                for (int i = 0; i < rows.Data(); i++)
                {
                    request.DisplayMessage = $"Введите количество элементов для {i + 1}'й строки: ";
                    
                    if((bufferedSize = request
                           .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
                               sizeValidator, 
                               sendRejectMessage: false)
                           .As<ConsoleResponseData<int>>()) != true)
                        return;

                    buffer[i] = new ConsoleResponseData<int[]>(new int[bufferedSize.Data()]);
                }
                
                ConsoleResponseData<int>[] borders = 
                    new ConsoleResponseData<int>[2];

                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = new ConsoleDataRequest<int>(
                            $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ")
                        .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), 
                            new ConsoleDataValidator<int>(
                                (data) =>
                                {
                                    if (i == 0)
                                        return true;

                                    return data > borders[0].Data();
                                }, "значение правой границы должно быть больше левой"),
                            false)
                        .As<ConsoleResponseData<int>>();
            
                    if(!borders[i]) return;
                }
                
                Random random = new Random();
                
                for (int i = 0; i < buffer.Length; i++)
                    for(int j = 0; j < buffer[i].Data().Length; j++)
                        buffer[i].Data()[j] = random.Next(borders[0].Data(), borders[1].Data());
                
                break;
        }

        IntArray = buffer;
    }
    
    public void TaskExpression()
    {
        if(IntArray.Length == 0)
            return;

        ConsoleResponseData<int[]>[] buffer = 
            new ConsoleResponseData<int[]>[IntArray.Length + 1];

        ConsoleArrayDataConverter<int> converter = BaseTypeArrayDataConverterFactory
            .MakeIntArrayConverter(delimiter: ";");

        for (int i = 0, a = 0; i < buffer.Length; i++)
        {
            if (i == 0)
            {
                buffer[i] = new ConsoleDataRequest<int[]>(
                        $"Введите множество целых чисел (через {converter.Delimiter}): \n")
                    .Request(converter)
                    .As<ConsoleResponseData<int[]>>();
                
                
                if(!buffer[i])
                    return;
                
                continue;
            }

            buffer[i] = IntArray[a++];
        }

        IntArray = buffer;
        
        Target.Output.WriteLine("\nСтрока добавлена");
    }

    public void OutputData()
    {
        for (int i = 0; i < IntArray.Length; i++)
        {
            for (int j = 0; j < IntArray[i].Data().Length; j++)
            {
                Target.Output.Write(
                    $"{IntArray[i].Data()[j]}");
                
                if(j + 1 != IntArray[i].Data().Length)
                    Target.Output.Write("\t");
            }
            
            Target.Output.WriteLine();
        }
        
        if(IntArray.Length == 0)
            Target.Output.WriteLine("Массив пуст");
    }
}