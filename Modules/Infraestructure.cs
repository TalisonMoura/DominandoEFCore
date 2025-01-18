using DominandoEFCore.Data;

namespace DominandoEFCore.Modules;

public class Infraestructure
{
    private readonly ApplicationDbContext _dbContext;

    public Infraestructure()
    {
        _dbContext = new();
        //GetDepartments();
        SensitiveData();
    }

    void GetDepartments()
    {
        using var db = _dbContext;
        var departments = db.Departments.Where(x => x.Id != Guid.Empty).ToList();
    }

    void SensitiveData()
    {
        using var db = _dbContext;
        var departments = db.Departments.Where(x => x.Description == "Department").ToList();
    }
}