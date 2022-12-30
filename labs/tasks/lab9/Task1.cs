using IO.converters;
using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.lab9.src;
using labs.utils;

namespace labs.lab9;

public sealed class Task1 : LabTask
{
    public readonly ConsoleTarget Target = new();

    private ConsoleResponseData<TimeArray> Times { get; set; }

    public Task1(string name = "lab9.task1", string description = "") : 
        base(1, name, description)
    {
        Times = new ConsoleResponseData<TimeArray>(new TimeArray(0));
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод множества объектов Time")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Ввод множества объектов Time [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Добавление объекта Time")
                .ExecuteAction(AddItem)
                .Build(),
            
            new LabTaskActionBuilder().Id(4).Name("Удаление объекта Time")
                .ExecuteAction(RemoveItem)
                .Build(),
            
            new LabTaskActionBuilder().Id(5).Name("Добавление/Вычитание минут к I-му объекту Time")
                .ExecuteAction(SetMinutesToTimeObject)
                .Build(),

            new LabTaskActionBuilder().Id(6).Name("Добавление времени к I-му объекту Time")
                .ExecuteAction(AddTimeToTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Id(7).Name("Вычитание времени из I-го объекта Time")
                .ExecuteAction(RemoveTimeFromTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Id(8).Name("Вывод максимального объекта Time")
                .ExecuteAction(DisplayMaxTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Id(9).Name("Вывод множества объектов Time")
                .ExecuteAction(OutputData)
                .Build()
        };

    }

    public void InputData(ArrayGenerationType type)
    {
        SimpleConsoleArrayDataConverter<Time> converter = ConsoleDataConverterFactory
            .MakeSimpleArrayConverter(ConsoleDataConverterFactory
                .MakeSimpleConverter<Time>(Time.TryParse), delimiter: ";");

        if (type == ArrayGenerationType.UserInput)
        {
            ConsoleResponseData<Time[]> data = (ConsoleResponseData<Time[]>)
                new ConsoleArrayDataRequest<Time>(
                    $"Введите множество 'hh:mm' (через '{converter.Delimiter}'): \n")
                .Request(converter);

            if (data.Code == (int)ConsoleResponseDataCode.ConsoleOk)
                Times = (ConsoleResponseData<TimeArray>)
                    new ConsoleResponseDataBuilder<TimeArray>()
                        .Code(data.Code)
                        .Data(new TimeArray(data.Data))
                        .Build();
            
            return;
        }

        ConsoleResponseData<int> size = (ConsoleResponseData<int>)
            new ConsoleDataRequest<int>("Введите количество элементов массива: ")
                .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
                    new ConsoleDataValidator<int>(data => data > 0, 
                        "ожидалось значение больше 0"));
        
        if(size.Code != (int)ConsoleResponseDataCode.ConsoleOk)
            return;

        Random random = new Random();

        Times.Data = new TimeArray(size.Data);

        for (int i = 0; i < Times.Data.Count; i++)
            Times.Data[i] = new Time(random.Next(0, Int32.MaxValue / 2));
    }

    public void AddItem()
    {
        ConsoleResponseData<Time> buffer = (ConsoleResponseData<Time>)
            new ConsoleDataRequest<Time>("Введите время (формат hh:mm): ")
                .Request(ConsoleDataConverterFactory.MakeSimpleConverter<Time>(Time.TryParse));
        
        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        Times.Data.Add(buffer.Data);
    }

    public void RemoveItem()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Введите номер элемента [1; {Times.Data.Count}]: ", 
            GetIndexValidator());

        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        Times.Data.RemoveAt(buffer.Data - 1);
    }
    
    public void SetMinutesToTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Введите номер элемента [1; {Times.Data.Count}]: ", 
            GetIndexValidator());
        
        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        ConsoleResponseData<int> minutes = 
            RequestIntData("Введите количество минут [+/- - добавить/вычесть]: ", 
                GetMinuteValidator(Times.Data[buffer.Data - 1]), false);
        
        if(minutes.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        Times.Data[buffer.Data - 1] += minutes.Data;
    }

    public void DisplayMaxTimeObject()
    {
        Time? max = Times.Data.Max();

        if (max == null)
        {
            Target.Write("Ошибка: Не удалось получить максимальный объект типа Time \n");
            return;
        }
        
        Target.Write($"Максимальное значение: {max}");
    }
    
    public void RemoveTimeFromTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Введите номер элемента [1; {Times.Data.Count}]: ", 
            GetIndexValidator());
        
        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        ConsoleResponseData<Time> time = (ConsoleResponseData<Time>)
                new ConsoleDataRequest<Time>("Введите отнимаемое время (формат hh:mm): ")
                    .Request(ConsoleDataConverterFactory.MakeSimpleConverter<Time>(Time.TryParse),
                        new ConsoleDataValidator<Time>(
                            data => data.AsSeconds() <= Times.Data[buffer.Data - 1].AsSeconds(), 
                $"не может превышать '{Times.Data[buffer.Data - 1]}'"), false
                    );
        
        if(time.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        Times.Data[buffer.Data - 1] -= time.Data;
    }
    
    public void AddTimeToTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Выберите объект [1; {Times.Data.Count}]: ", 
            GetIndexValidator());
        
        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        ConsoleResponseData<Time> time = (ConsoleResponseData<Time>)
            new ConsoleDataRequest<Time>("Введите добавочное время (формат hh:mm): ")
                .Request(ConsoleDataConverterFactory
                    .MakeSimpleConverter<Time>(Time.TryParse), sendRejectMessage: false);
        
        if(time.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        Times.Data[buffer.Data - 1] += time.Data;
    }

    private ConsoleResponseData<int> 
        RequestIntData(string message, IValidatableData<int>? validator = default, bool send = true)
    {
        OutputData();
        
        Target.Write("\n");
        
        return (ConsoleResponseData<int>) new ConsoleDataRequest<int>(message)
                .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), validator, send);
    }

    private IValidatableData<int> GetIndexValidator()
    {
        return new ConsoleDataValidator<int>(
            data => data > 0 && data <= Times.Data.Count,
            "выход за допустимые границы");
    }

    private IValidatableData<int> GetMinuteValidator(Time obj)
    {
        return new ConsoleDataValidator<int>(data =>
        {
            if (data > 0)
                return true;
            
            return Math.Abs(data) <= obj.AsSeconds() / 60;
        }, $"значение не может превышать '{obj.AsSeconds() / 60}'");
    }

    public void OutputData()
    {
        for (int i = 0; i < Times.Data.Count; i++)
            Target.Write($"{i + 1}: {Times.Data[i]} \n");
        
        if(Times.Data.Count == 0)
            Target.Write("Массив пуст \n");
    }
}