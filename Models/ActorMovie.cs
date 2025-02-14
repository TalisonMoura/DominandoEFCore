namespace DominandoEFCore.Models;

public class ActorMovie
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public Guid MovieId { get; set; }

    public Actor Actor { get; set; }
    public Movie Movie { get; set; }

    public ActorMovie(Guid actorId, Guid movieId)
    {
        MovieId = movieId;
        ActorId = actorId;
    }
}

public class Actor
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<ActorMovie> Movies { get; set; } = [];
}

public class Movie
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public ICollection<ActorMovie> Actors { get; set; } = [];
}