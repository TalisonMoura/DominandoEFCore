using DominandoEFCore.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {       
        const string connectionString = "Server=TALISONJM\\SQLEXPRESS;Database=DominandoEfCore;Integrated Security=true;TrustServerCertificate=True;pooling=true";

        optionsBuilder.UseSqlServer(connectionString)
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}
