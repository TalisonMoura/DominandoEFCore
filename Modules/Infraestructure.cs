using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules;

public class Infraestructure
{
    private readonly ApplicationDbContext _dbContext;

    public Infraestructure()
    {
        _dbContext = new();
        //GetDepartments();
        //SensitiveData();
        //EnableBatchSize();
        GeneralCommand();
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

    void EnableBatchSize()
    {
        using var db = _dbContext;
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        for (int i = 0; i < 50; i++)
            db.Departments.Add(new() { Description = $"Department {i}" });

        db.SaveChanges();
    }

    void GeneralCommand()
    {
        using var db = _dbContext;
        db.Database.SetCommandTimeout(10);
        db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'; SELECT 1");
    }
}