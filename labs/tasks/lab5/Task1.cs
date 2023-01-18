using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab5;

public sealed class Task1 :
    LabTask
{
    public ConsoleResponseData<int[]> IntArray
    {
        get; 
        private set;
    }

    public Task1(string name = "lab5.task1", string description = "") : 
        base(1, name, description)
    {
        IntArray = new ConsoleResponseData<int[]>(Array.Empty<int>());
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Name("Создать массив")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Name("Создать массив [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Name("Удалить все нечетные элементы")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        ConsoleArrayDataConverter<int> converter =
            BaseTypeArrayDataConverterFactory.MakeIntArrayConverter(delimiter: ";");

        if (type == ArrayGenerationType.UserInput)
        {
            ConsoleResponseData<int[]> buffer = new ConsoleDataRequest<int[]>(
                    $"Введите множество целых чисел (через '{converter.Delimiter}'):")
                .Request(converter)
                .As<ConsoleResponseData<int[]>>();

            if (lab1.Task1.TryReceiveWithNotify(ref buffer, buffer, true))
                IntArray = buffer;

            return;
        }

        ConsoleDataRequest<int> request = 
            new ConsoleDataRequest<int>("Введите размер массива: ");

        ConsoleResponseData<int> size = request
            .Request(converter.ElementConverter, new ConsoleDataValidator<int>(
                (data) => data > 0, "значение должно быть больше 0"))
            .As<ConsoleResponseData<int>>();

        if(!lab1.Task1.TryReceiveWithNotify(ref size, size))
            return;
        
        ConsoleResponseData<int>[] borders = 
            new ConsoleResponseData<int>[2];

        for (int i = 0; i < borders.Length; i++)
        {
            request.DisplayMessage = 
                $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ";

            var i1 = i;
            borders[i] = request
                .Request(converter.ElementConverter, new ConsoleDataValidator<int>(
                        (data) =>
                        {
                            if (i1 == 0)
                                return true;

                            return data > borders[0].Data();
                        }, "значение правой границы должно быть больше левой"),
                    false)
                .As<ConsoleResponseData<int>>();

            if(!lab1.Task1.TryReceiveWithNotify(ref borders[i], borders[i], i != 0)) 
                return;
        }

        IntArray.Data(new int[size.Data()]);
        
        Random random = new Random();

        for (int i = 0; i < IntArray.Data().Length; i++)
            IntArray.Data()[i] = random.Next(borders[0].Data(), borders[1].Data());
    }

    // удаление нечетных элементов
    public void TaskExpression()
    {
        if(lab4.Task1.IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создаение массива"))
            return;
        
        IntArray.Data(IntArray.Data().Where(x => (x % 2) == 0).ToArray());
        
        OutputData();
    }
    
    public void OutputData()
    {
        for (int i = 0; i < IntArray.Data().Length; i++)
            Target.Output.WriteLine($"{i + 1}: {IntArray.Data()[i]}");
        
        if(IntArray.Data().Length == 0)
            Target.Output.WriteLine("Массив пуст");
    }
}