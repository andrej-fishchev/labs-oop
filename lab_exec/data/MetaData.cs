using labs.builders;
using labs.entities;
using labs.lab1;

namespace lab_exec;

public static class MetaData
{
    public static readonly List<ILabEntity<int>> LabList = new()
    {
        new LabBuilder()
            .Name("Лабораторная работа №1")
            .Description("Организация ввода и вывода данных")
            .Tasks(new List<LabTask>
            {
                new Task1("Вычислить значение выражения: m - ++n", "Вариант №24"),
                new Task2("Вычислить значение выражения: m++ > --n", "Вариант №24"),
                new Task3("Вычислить значение выражения: m-- < ++n", "Вариант №24"),
                new Task4("Вычислить значение выражения: arcsin(|x+1|)", "Вариант №24"),
                new Task5("Принадлежность точки заштрихованной области", "Вариант №24"),
                new Task6("Тип данных с плавающей точкой", "Вариант №24")
            }).Build(),
        
        new LabBuilder()
            .Name("Лабораторная работа №3")
            .Description("Вычисление функций с использованием их разложения в степенной ряд")
            .Build(),
        
        new LabBuilder()
            .Name("Лабораторная работа №4")
            .Description("Работа с одномерными массивами")
            .Tasks(new List<LabTask>
            {
                new labs.lab4.Task1("Работа с целочисленным одномерным массивом", "Вариант №24")
            }).Build(),
        
        new LabBuilder()
            .Name("Лабораторная работа №5")
            .Description("Динамические массивы различных типов")
            .Tasks(new List<LabTask>()
            {
                new labs.lab5.Task1("Действия над одномерным массивом", "Вариант №24"),
                new labs.lab5.Task2("Действия над двумерным массивом", "Вариант №24"),
                new labs.lab5.Task3("Действия над рваным массивом", "Вариант №24")
            })
            .Build(),
        
        new LabBuilder()
            .Name("Лабораторная работа №6")
            .Description("Класс Array. Строки. Класс String")
            .Tasks(new List<LabTask>()
            {
                new labs.lab6.Task1("Сортировка элементов рванного массива", "Вариант №23"),
                new labs.lab6.Task2("Переворот и сортировка слов предложения", "Вариант №24")
            })
            .Build(),
        
        new LabBuilder()
            .Name("Лабораторная работа №9")
            .Description("Классы и объекты")
            .Tasks(new List<LabTask>()
            {
                new labs.lab9.Task1("Реализация класса Time и TimeArray", "Вариант №14")
            })
            .Build()
    };
}