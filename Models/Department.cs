using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;

    public List<Employee> Employees { get; set; }
}