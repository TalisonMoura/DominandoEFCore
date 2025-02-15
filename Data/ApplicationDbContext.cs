using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Actor> Actors { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Converter> Converters { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<ActorMovie> ActorMovies { get; set; }

    public DbSet<Dictionary<string, object>> Configurations => Set<Dictionary<string, object>>("Configurations");

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
        //modelBuilder.Entity<Department>().Property(x => x.Description).WithCollation(ECollationType.UnSentitive); // collation being used at a spesific property

        //modelBuilder.HasSequence<int>("MySequence", "sequences").StartsAt(1).IncrementsBy(2).HasMin(1).HasMax(100).IsCyclic();

        /*modelBuilder.Entity<Department>()
                    .HasIndex(i => new { i.Description, i.IsActive })
                    .HasDatabaseName("idx_my_compost_index")
                    .HasFilter("Descriptions IS NOT NULL")
                    .HasFillFactor(80)
                    .IsUnique(false); */// creating a compost index to handle the queries with more performance

        //modelBuilder.Entity<State>().HasData([new() { Id = Guid.NewGuid(), Name = "Minas Gerais" }, new() { Id = Guid.NewGuid(), Name = "Sergipe" }]);

        //modelBuilder.HasDefaultSchema("registers");

        //modelBuilder.Entity<State>().ToTable("States", "SecondScheme");

        //var conversion = new ValueConverter<Models.Version, string>(x => x.ToString(), x => (Models.Version)Enum.Parse(typeof(Models.Version), x));

        //modelBuilder.Entity<Converter>()
        //    .Property(x => x.Version)
        //    .HasConversion(conversion);
        ////.HasConversion(new EnumToStringConverter<Models.Version>()); *** Some ways to create your converter data ***
        ////.HasConversion(x => x.ToString(), x => (Models.Version)Enum.Parse(typeof(Models.Version), x));
        ////.HasConversion<string>();

        //modelBuilder.Entity<Converter>()
        //    .Property(x => x.Status)
        //    .HasConversion(new CustomConverter());

        //modelBuilder.Entity<Department>()
        //    .Property<DateTime>("LastUpdate");

        // two diffent ways to register your entity type configuration class
        //modelBuilder.ApplyConfiguration(new ClientConfigurations());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configurations", c =>
        {
            c.Property<Guid>("Id");

            c.Property<string>("Key")
             .HasColumnType("varchar(40)")
             .IsRequired();

            c.Property<string>("Value")
             .HasColumnType("varchar(255)")
             .IsRequired();
        });
    }
}