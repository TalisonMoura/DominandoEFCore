namespace DominandoEFCore.Models;

public class Department
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }

    public List<Employee> Employees { get; set; }
}