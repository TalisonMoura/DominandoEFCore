using DominandoEFCore.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore;

public class Program
{
    private static void Main(string[] args)
    {
        //EnsureCreatedAndDeleted();
        GanEnsureCreated();
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
}