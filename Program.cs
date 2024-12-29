using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore;

public class Program
{
    private static void Main(string[] args)
    {
        HealthCheckOnDataBse();
        //GanEnsureCreated();
        //EnsureCreatedAndDeleted();
    }

    static void EnsureCreatedAndDeleted()
    {
        using var db = new ApplicationDbContext();
        //db.Database.EnsureCreated();
        db.Database.EnsureDeleted();
    }

    static void GanEnsureCreated()
    {
        using var dbDefault = new ApplicationDbContext();
        using var dbCity = new ApplicationDbContextCity();

        dbDefault.Database.EnsureCreated();
        dbCity.Database.EnsureCreated();

        var databaseCreator = dbCity.GetService<IRelationalDatabaseCreator>();
        databaseCreator.CreateTables();
    }

    static void HealthCheckOnDataBse()
    {
        using var db = new ApplicationDbContext();
        var canConnect = db.Database.CanConnect();

        if (canConnect ) 
            Console.WriteLine("I'm able to connect");
        else
            Console.WriteLine("I'm not able to connect");
    }
}