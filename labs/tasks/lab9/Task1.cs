using IO.converters;
using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
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
        Times = new ConsoleResponseData<TimeArray>(new TimeArray());
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод множества объектов Time")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Ввод множества объектов Time [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Добавление объекта Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(4).Name("Удаление объекта Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(5).Name("Добавление минут к I-му объекту Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(6).Name("Вычитание минут из I-го объекта Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(7).Name("Добавление времени к I-му объекту Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(8).Name("Вычитание времени из I-го объекта Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(9).Name("Вывод максимального объекта Time")
                .ExecuteAction(() => {})
                .Build(),
            
            new LabTaskActionBuilder().Id(10).Name("Вывод множества объектов Time")
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
        
        
    }

    public void OutputData()
    {
        for (int i = 0; i < Times.Data.Count; i++)
        {
            
        }
    }
}