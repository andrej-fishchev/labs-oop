using labs.builders;
using labs.entities;

namespace labs.src.lab1;

public sealed class Task1 :
    LabTask<int>
{
    public Task1(string name = "lab1.task1", string description = "") 
        : base(1, name)
    {
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .Delegator(() => {})
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .Delegator(() => {})
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .Delegator(() => {})
                .Build<LabTaskAction<int>>()
        };
    }
}