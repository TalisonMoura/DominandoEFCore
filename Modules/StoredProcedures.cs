using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules;

public class StoredProcedures
{
    public StoredProcedures()
    {
        //CreateProcedure();
        //InsertIntoByProcedure();
        //CreateProcedureForGet();
        GetDepartmentsByProcedure();
    }

    static ApplicationDbContext GetContext() => new();

    static void CreateProcedure()
    {
        var createDepartment = @"
        CREATE OR ALTER PROCEDURE CreateDepartment
            @Description varchar(50),
            @IsActive bit
        AS
        BEGIN
            INSERT INTO
                Departments(Id, Description, IsActive) VALUES (NEWID(), @Description, @IsActive)
        END
        ";

        using var db = GetContext();
        db.Database.ExecuteSqlRaw(createDepartment);
    }

    static void InsertIntoByProcedure()
    {
        using var db = GetContext();
        db.Database.ExecuteSqlRaw("execute CreateDepartment @p0, @p1", "Department by procedure", true);
    }

    static void CreateProcedureForGet()
    {
        var createGetDepartments = @"
        CREATE OR ALTER PROCEDURE GetDepartments
            @Description varchar(50)
        AS
        BEGIN
            SELECT * FROM Departments WHERE Description like @Description + '%'
        END
        ";

        using var db = GetContext();
        db.Database.ExecuteSqlRaw(createGetDepartments);
    }

    static void GetDepartmentsByProcedure()
    {
        using var db = GetContext();
        
        var departments = db.Departments.FromSqlRaw("execute GetDepartments @p0", "Depart").ToList();

        departments?.ForEach(d => Console.WriteLine($"{d.Description}"));
    }
}