using DominandoEFCore.Data;

namespace DominandoEFCore.Modules;

public class Infraestructure
{
    private readonly ApplicationDbContext _dbContext;

    public Infraestructure()
    {
        _dbContext = new();
        GetDepartments();


    }

    void GetDepartments()
    {
        using var db = _dbContext;
        var departments = db.Departments.Where(x => x.Id != Guid.Empty).ToList();
    }


}