using DominandoEFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DominandoEFCore.Data.Configurations;

public class ActorMovieConfiguration : IEntityTypeConfiguration<ActorMovie>
{
    public void Configure(EntityTypeBuilder<ActorMovie> builder)
    {
        builder
            .HasOne(x => x.Movie)
            .WithMany(x => x.Actors)
            .HasForeignKey(x => x.MovieId);

        builder
            .HasOne(x => x.Actor)
            .WithMany(x => x.Movies)
            .HasForeignKey(x => x.ActorId);
    }
}
