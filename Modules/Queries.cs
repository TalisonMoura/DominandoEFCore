using DominandoEFCore.Data;
using DominandoEFCore.Models;

namespace DominandoEFCore.Modules;

public class Queries
{
    public Queries()
    {
        ProjectedQeuerie();
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
}