using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .ToTable("Persons")
            .HasDiscriminator<int>("PersonTipe")
            .HasValue<Person>(3)
            .HasValue<Instructor>(6)
            .HasValue<Student>(99);
    }
}