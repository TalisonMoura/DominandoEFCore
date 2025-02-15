namespace DominandoEFCore.Models;

public class Person
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class Instructor : Person
{
    public DateTime Since { get; set; }
    public string Tecnology { get; set; }
}

public class Student : Person
{
    public int Age { get; set; }
    public DateTime ContractDate { get; set; }
}