using System.Text;

namespace labs.tasks.lab10.src;

public class Student :
    IDescribe,
    ICloneable
{
    public string Name { get; set; }
    
    public int Age { get; set; }

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append("Студент: \n")
            .Append($"Имя: {Name}\n")
            .Append($"Возраст: {Age}\n")
            .ToString();
    }

    public object Clone()
    {
        return new Student(Name, Age);
    }
    
    public string Describe()
    {
        return ToString();
    }
}