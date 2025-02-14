namespace DominandoEFCore.Models;

public class State
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public Goverment Goverment { get; set; }
}

public class Goverment
{
    public Guid Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public string Team { get; set; }

    public Guid StateId { get; set; }
    public State State { get; set; }
}