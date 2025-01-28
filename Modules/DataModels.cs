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
            WorkingWithShadowProperty();
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
    }
}