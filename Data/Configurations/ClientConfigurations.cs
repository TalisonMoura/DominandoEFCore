using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Data.Configurations;

public class ClientConfigurations : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(x => x.NeiborHood).HasColumnName("NeiborHood");
            address.ToTable("Address");
        });
    }
}