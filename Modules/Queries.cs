using DominandoEFCore.Data;
using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules;

public class Queries
{
    public Queries()
    {
        //ProjectedQeuerie();
        //QeuerieRaw();
        //QeuerieInterpolated();
        //QeuerieIWithTag();
        Qeuerie1NN1();
    }

    static ApplicationDbContext GetContext() => new();

    static void SetupLoadingType(ApplicationDbContext context)
    {
        if (!context.Departments.Any())
        {
            context.Departments.AddRange
            (
                new Department { Description = "Department 1", Employees = [new() { Name = "Talison Moura", Cpf = "00000000001", Rg = "MG00000001" }] },
                new Department { Description = "Department 2", Employees = [new() { Name = "Raphael Moura", Cpf = "00000000011", Rg = "MG00000011" }, new() { Name = "Marcio Moura", Cpf = "00000000111", Rg = "MG00000111" }] }
            );

            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }

    static void ProjectedQeuerie()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.Where(x => x.Id != Guid.Empty).Select(x => new { x.Description, Employess = x.Employees.Select(x => x.Name).ToList() }).ToList();

        departments?.ForEach(x =>
        {
            Console.WriteLine($"Description: {x.Description}");
            x.Employess.ForEach(e => Console.WriteLine($"\t Name: {e}"));
        });
    }

    static void QeuerieRaw()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.FromSqlRaw("select * from Departments").ToList();

        departments?.ForEach(x => Console.WriteLine($"Description: {x.Description}"));
    }

    static void QeuerieInterpolated()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.FromSqlInterpolated($"select * from Departments D where D.Id != {Guid.Empty}").ToList();

        departments?.ForEach(x => Console.WriteLine($"Description: {x.Description}"));
    }

    static void QeuerieIWithTag()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.TagWith("I'm just testing the server").ToList();

        departments?.ForEach(x => Console.WriteLine($"Description: {x.Description}"));
    }

    static void Qeuerie1NN1()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.Include(x => x.Employees).ToList();
        departments?.ForEach(x => x.Employees.ForEach(e => Console.WriteLine($"Description: {e.Name}")));

        var employess = db.Employees.Include(x => x.Department).ToList();
        employess?.ForEach(x => Console.WriteLine($"Description: {x.Name}, Department: {x.Department.Description}"));
    }


}