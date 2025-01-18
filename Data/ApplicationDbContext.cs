using DominandoEFCore.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DominandoEFCore.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    private readonly StreamWriter _streamer = new("T:\\ProjetosEstudo\\ProjetosC#\\DominandoEFCore\\ef_core_log.txt", append: true);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Server=TALISONJM\\SQLEXPRESS;Database=DominandoEfCore;Integrated Security=true;TrustServerCertificate=True;pooling=true";

        optionsBuilder.UseSqlServer(connectionString, ctxOptsBuilder => { ctxOptsBuilder.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null); ctxOptsBuilder.MaxBatchSize(100).CommandTimeout(5); })
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
    }

    public override void Dispose()
    {
        base.Dispose();
        _streamer.Dispose();
    }
}
