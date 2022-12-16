using labs.abstracts;
using labs.builders;
using labs.entities;
using labs.interfaces;
using labs.lab1;

namespace lab_exec;

public static class MetaData
{
    public static List<ILabEntity<int>> LabList = new()
    {
        new LabBuilder()
            .Id(1)
            .Name("Ввод и вывод")
            .Tasks(new List<LabTask>
            {
                new Task1(),
                new Task2(),
                new Task3(),
                new Task4(),
                new Task5(),
                new Task6()
            }).Build(),
        
        new LabBuilder()
            .Id(3)
            .Name("Рекурентные соотношения")
            .Build(),
        
        new LabBuilder()
            .Id(4)
            .Name("Массивы")
            .Tasks(new List<LabTask>
            {
                new labs.lab4.Task1()
            }).Build()
    };
}