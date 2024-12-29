using System.Diagnostics;
using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore;

public class Program
{
    public static int Count { get; set; }

    private static void Main(string[] args)
    {
        //EnsureCreatedAndDeleted();
        //GapEnsureCreated();
        //HealthCheckOnDataBse();

        //warmup
        GetContext().Departments.AsNoTracking().Any();
        Count = 0;
        HandleConnectionState(false);
        Count = 0;
        HandleConnectionState(true);
    }

    static void EnsureCreatedAndDeleted()
    {
        using var db = GetContext();
        //db.Database.EnsureCreated();
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

    static ApplicationDbContext GetContext() => new();
}