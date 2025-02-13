namespace DominandoEFCore.Models;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string CellPhone { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; }
    public string NeiborHood { get; set; }
    public string City { get; set; }
    public string State { get; set; }
}