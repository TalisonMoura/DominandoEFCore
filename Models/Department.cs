using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore.Models;

public class Department
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;

    public Department()
    {

    }

    //private ILazyLoader LazyLoader { get; set; }
    //private Department(ILazyLoader lazyLoader)
    //{
    //    LazyLoader = lazyLoader;
    //}
    //private List<Employee> _employees;
    //public List<Employee> Employees
    //{
    //    get => LazyLoader.Load(this, ref _employees);
    //    set => _employees = value;
    //}// Para esse cenário, o Lazy Loading é a melhor opção, pois não é necessário carregar os funcionários de um departamento a todo momento, apenas quando necessário.

    private Action<object, string> LazyLoarder { get; set; }
    private Department(Action<object, string> lazyLoarder)
    {
        LazyLoarder = lazyLoarder;
    }

    private List<Employee> _employees;
    public List<Employee> Employees
    {
        get { LazyLoarder?.Invoke(this, nameof(Employees)); return _employees; }
        set => _employees = value;
    }
}