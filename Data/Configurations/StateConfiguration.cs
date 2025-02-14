﻿using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Data.Configurations;

public class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder
            .HasOne(s => s.Goverment)
            .WithOne(g => g.State)
            .HasForeignKey<Goverment>(g => g.StateId);

        builder.Navigation(x => x.Goverment).AutoInclude();
    }
}