using System.Text.Json;
using DominandoEFCore.Data;
using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore.Modules
{
    public class DataModels
    {
        ApplicationDbContext _context;

        public DataModels()
        {
            _context = new();
            //Collations();
            //DataPropagation();
            //ValueConverter();
            //CustomConverter();
            //WorkingWithShadowProperty();
            //PropertiesType();
            //Relationship1To1();
            //Relationship1ToN();
            //RelationshipNtoN();
            //BackingField();
            //TPHExample();
            PropertiesPackage();
        }


        ApplicationDbContext Ensures()
        {
            var db = _context;
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            return db;
        }

        void Collations()
        {
            using var db = Ensures();
        }

        void DataPropagation()
        {
            using var db = Ensures();
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        void Scheme()
        {
            using var db = _context;
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        void ValueConverter() => Scheme();

        void CustomConverter()
        {
            using var db = Ensures();

            db.Converters.Add(new() { Status = Models.Status.Returned });

            db.SaveChanges();

            var inAnalisysConverter = db.Converters.AsNoTracking().FirstOrDefault(x => x.Status.Equals(Models.Status.InAnalysis));
            var returnedConverter = db.Converters.AsNoTracking().FirstOrDefault(x => x.Status.Equals(Models.Status.Returned));
        }

        void WorkingWithShadowProperty()
        {
            using var db = Ensures();

            //var department = new Department() { Description = "Shadow Property Department" };

            //db.Departments.Add(department);

            //db.Entry(department).Property("LastUpdate").CurrentValue = DateTime.Now;

            //db.SaveChanges();

            var departments = db.Departments.Where(x => EF.Property<DateTime>(x, "LastUpdate") < DateTime.Now).ToArray();
        }

        void PropertiesType()
        {
            using var db = Ensures();

            db.Clients.Add(new Client() { Name = "Talison de Jesus Moura", CellPhone = "31980120850", Address = new() { NeiborHood = "Aparecida", City = "Belo Horizonte" } });

            db.SaveChanges();

            var clients = db.Clients.AsNoTracking().ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };

            foreach (var client in clients)
                Console.WriteLine(JsonSerializer.Serialize(client, options));
        }

        void Relationship1To1()
        {
            using var db = Ensures();

            db.States.Add(new State { Name = "Minas Gerais", Goverment = new() { Name = "Talison de Jesus Moura" } });

            db.SaveChanges();

            var states = db.States.AsNoTracking().ToList();

            foreach (var state in states)
                Console.WriteLine($"State: {state.Name}, Goverment: {state.Goverment.Name}");

        }

        void Relationship1ToN()
        {
            using var db = Ensures();

            db.States.Add(new State { Name = "Minas Gerais", Goverment = new() { Name = "Talison de Jesus Moura" } }.AddCitie(new() { Name = "Belo Horizonte" }));

            db.SaveChanges();

            using var database = _context;

            var states = database.States.ToList();

            states[0].AddCitie(new() { Name = "Contagem" });

            database.SaveChanges();

            foreach (var state in database.States.Include(x => x.Cities).AsNoTracking())
            {
                Console.WriteLine($"State: {state.Name}, Goverment: {state.Goverment.Name}");
                foreach (var city in state.Cities)
                    Console.WriteLine($"\t City: {city.Name}");
            }
        }

        void RelationshipNtoN()
        {
            using var db = Ensures();

            var actor1 = new Actor { Id = Guid.NewGuid(), Name = "Mourão" };
            var actor2 = new Actor { Id = Guid.NewGuid(), Name = "MouraMal" };
            var actor3 = new Actor { Id = Guid.NewGuid(), Name = "Taleco" };

            var movie1 = new Movie { Id = Guid.NewGuid(), Description = "Plin" };
            var movie2 = new Movie { Id = Guid.NewGuid(), Description = "Plon" };
            var movie3 = new Movie { Id = Guid.NewGuid(), Description = "Plun" };

            db.AddRange(actor1, actor2, actor3, movie1, movie2, movie3);

            actor1.Movies.Add(new(actor1.Id, movie1.Id));
            actor1.Movies.Add(new(actor1.Id, movie2.Id));

            actor2.Movies.Add(new(actor2.Id, movie1.Id));

            movie3.Actors.Add(new(actor1.Id, movie3.Id));
            movie3.Actors.Add(new(actor2.Id, movie3.Id));
            movie3.Actors.Add(new(actor3.Id, movie3.Id));

            db.AddRange(actor1, actor2, movie3);

            db.SaveChanges();

            foreach (var actor in db.Actors.Include(x => x.Movies))
            {
                Console.WriteLine($"Actor {actor.Name}");
                foreach (var movie in actor.Movies)
                    Console.WriteLine($"\t Movie: {movie.Movie.Description}");
            }
        }

        void BackingField()
        {
            using var db = Ensures();

            var document = new Document();
            document.SetCpf("12365498701");

            db.Documents.Add(document);
            db.SaveChanges();

            foreach (var doc in db.Documents.AsNoTracking())
                Console.WriteLine($"Cpf -> {doc.Cpf()}");
        }

        void TPHExample()
        {
            using var db = Ensures();

            var person = new Person { Name = "Language connect people" };

            var instructor = new Instructor { Name = "la vie in rose", Tecnology = ".Net", Since = DateTime.UtcNow };

            var student = new Student { Name = "Betllejuice", Age = 31, ContractDate = DateTime.UtcNow.AddDays(-1) };

            db.AddRange(person, instructor, student);
            db.SaveChanges();

            //var persons = db.Persons.AsNoTracking().ToArray();
            var persons = db.Persons.OfType<Student>().AsNoTracking().ToArray();

            var instructors = db.Instructors.AsNoTracking().ToArray();
            var students = db.Students.AsNoTracking().ToArray();

            Console.WriteLine("***** Persons *****");
            foreach (var p in persons)
                Console.WriteLine($"Id: {p.Id} -> {p.Name}");

            Console.WriteLine("***** Instructors *****");
            foreach (var i in instructors)
                Console.WriteLine($"Id: {i.Id} -> {i.Name}, Tecnology: {i.Tecnology}, Since: {i.Since}");

            Console.WriteLine("***** Students *****");
            foreach (var s in students)
                Console.WriteLine($"Id: {s.Id} -> {s.Name}, Age: {s.Age}, Contract Date: {s.ContractDate}");

        }

        void PropertiesPackage()
        {
            using var db = Ensures();

            var configuration = new Dictionary<string, object>
            {
                ["Key"] = "DataBasePassword",
                ["Value"] = Guid.NewGuid().ToString("D"),
            };

            db.Configurations.Add(configuration);
            db.SaveChanges();

            var configurations = db.Configurations.AsNoTracking().Where(p => (p["Key"] as string).Equals("DataBasePassword")).ToArray();
            foreach (var config in configurations)
                Console.WriteLine($"Key: {config["Key"]} - Value: {config["Value"]}");
        }
    }
}