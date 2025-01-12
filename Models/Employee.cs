namespace DominandoEFCore.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string Rg { get; set; }
    public string Cpf { get; set; }
    public string Name { get; set; }

    public Guid DepartmentId { get; set; }
    public virtual Department Department { get; set; }
}