using DominandoEFCore.Data;
using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
            Relationship1ToN();
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
    }
}