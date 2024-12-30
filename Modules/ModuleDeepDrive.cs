using DominandoEFCore.Data;
using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules;

public class ModuleDeepDrive
{
    public ModuleDeepDrive()
    {
        AdvanceLoading();
        ExplicitLoading();
    }

    static ApplicationDbContext GetContext() => new();

    static void AdvanceLoading()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.Include(x => x.Employees);
        foreach (var department in departments)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine($"Department: {department.Description}");

            if (department.Employees?.Count > 0)
                foreach (var employee in department.Employees)
                    Console.WriteLine($"\tEmployee: {employee.Name}");
            else
                Console.WriteLine("\t No employees were found");
        }
    }

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

    static void ExplicitLoading()
    {
        using var db = GetContext();
        SetupLoadingType(db);

        var departments = db.Departments.ToList();

        foreach (var department in departments)
        {
            if (department.Id != Guid.NewGuid())
                _ = db.Entry(department).Collection(d => d.Employees).Query().Where(e => e.Id != Guid.Empty).ToList();

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine($"Department: {department.Description}");

            if (department.Employees?.Count > 0)
                foreach (var employee in department.Employees)
                    Console.WriteLine($"\tEmployee: {employee.Name}");
            else
                Console.WriteLine("\t No employees were found");
        }
    }
}