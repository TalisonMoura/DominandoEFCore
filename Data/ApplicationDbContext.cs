using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString = "Server=TALISONJM\\SQLEXPRESS;Database=DominandoEfCore;Integrated Security=true;TrustServerCertificate=True;pooling=true";

        optionsBuilder
            .UseSqlServer(connectionString, ctxOptsBuilder => ctxOptsBuilder.EnableRetryOnFailure(maxRetryCount: 2, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI"); // collation being used as global case
        modelBuilder.Entity<Department>().Property(x => x.Description).WithCollation(ECollationType.UnSentitive); // collation being used at a spesific property

        modelBuilder.HasSequence<int>("MySequence", "sequences").StartsAt(1).IncrementsBy(2).HasMin(1).HasMax(100).IsCyclic();
    }
}

public enum ECollationType
{
    UnSentitive,
    UnSentitiveCaseSensitiveAccent,
    SentitiveCaseUnsensitiveAccent,
}

public static class EntityFrameworkExtensions
{
    public static PropertyBuilder<string> WithCollation(this PropertyBuilder<string> property, ECollationType? type = null)
    {
        return property.UseCollation(type switch
        {
            ECollationType.UnSentitive => "SQL_Latin1_General_CP1_CI_AI",
            ECollationType.SentitiveCaseUnsensitiveAccent => "SQL_Latin1_General_CP1_CS_AI",
            ECollationType.UnSentitiveCaseSensitiveAccent => "SQL_Latin1_General_CP1_CI_AS",
            _ => "SQL_Latin1_General_CP1_CS_AS"
        });
    }
}
