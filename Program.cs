using System.Diagnostics;
using DominandoEFCore.Data;
using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore;

public class Program
{
    private static int Count { get; set; }

    private static void Main(string[] args)
    {
        //EnsureCreatedAndDeleted();
        //GapEnsureCreated();
        //HealthCheckOnDataBse();

        //warmup
        //GetContext().Departments.AsNoTracking().Any();
        //Count = 0;
        //HandleConnectionState(false);
        //Count = 0;
        //HandleConnectionState(true);

        //ExecuteSQL();
        //SqlInjection();
        //PendingMigrates();
        ExecutePendindMigrationsOnExecutionTime();
    }

    static ApplicationDbContext GetContext() => new();

    static void EnsureCreatedAndDeleted()
    {
        using var db = GetContext();
        db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
    }

    static void GapEnsureCreated()
    {
        using var dbDefault = GetContext();
        using var dbCity = new ApplicationDbContextCity();

        dbDefault.Database.EnsureCreated();
        dbCity.Database.EnsureCreated();

        var databaseCreator = dbCity.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

    static void HealthCheckOnDataBse()
    {
        using var db = GetContext();
        var canConnect = db.Database.CanConnect();

        if (canConnect)
            Console.WriteLine("I'm able to connect");
        else
            Console.WriteLine("I'm not able to connect");
    }

    static void HandleConnectionState(bool isHandleConnectionState)
    {
        using var db = GetContext();
        var time = Stopwatch.StartNew();

        var connection = db.Database.GetDbConnection();

        connection.StateChange += (_, _) => ++Count;

        if (isHandleConnectionState)
            connection.Open();

        for (int i = 0; i < 200; i++)
            db.Departments.AsNoTracking().Any();

        time.Stop();
        Console.WriteLine($"TotalTime: {time.Elapsed}, {isHandleConnectionState}, {Count}");
    }

    static void ExecuteSQL()
    {
        using var db = GetContext();
        using var cmd = db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SELECT 1";
        var rowsaffected = cmd.ExecuteNonQuery();

        Console.WriteLine($"Number of rows affected: {rowsaffected}");

        string rawCityName = "BH É NOISS";
        Guid cityId = Guid.Parse("55D7CF2C-A4A3-4D16-9B0E-55519CBFD57A");
        var rowsaffectedraw = db.Database.ExecuteSqlRaw("update Cities set Name={0} where Id={1}", rawCityName, cityId);
        Console.WriteLine($"Number of rows affected sqlraw: {rowsaffectedraw}");

        string interpolatedCityName = "BH É NOISS <3 PRAÇA SETE, FOTO NA HORA É FOTO";
        var rowsaffectedinterpolated = db.Database.ExecuteSqlInterpolated($"update Cities set Name={interpolatedCityName} where Id={cityId}");
        Console.WriteLine($"Number of rows affected sqlinterpolated: {rowsaffectedinterpolated}");
    }

    static void SqlInjection()
    {
        using var db = GetContext();
        db.Database.EnsureDeleted();
        GapEnsureCreated();

        db.Departments.AddRange
        (
            new Department { Description = "Department 1" },
            new Department { Description = "Department 2" }
        );

        db.SaveChanges();

        string description = "Teste ' or 1=' 1";
        var rowsaffectedraw = db.Database.ExecuteSqlRaw($"update Departments set Description='AttackSqlIjection' where Description='{description}'");

        foreach (var department in db.Departments.AsNoTracking())
            Console.WriteLine($"ID: {department.Id}, DESCRIPTION: {department.Description}");
    }

    static void PendingMigrates()
    {
        using var db = GetContext();
        var migratesPending = db.Database.GetPendingMigrations();
        Console.WriteLine($"Migrates pending: {string.Join("\n ", migratesPending)}, Total Migrates: {migratesPending.Count()}");
    }

    static void ExecutePendindMigrationsOnExecutionTime()
    {
        using var db = GetContext();
        db.Database.Migrate();
    }
}