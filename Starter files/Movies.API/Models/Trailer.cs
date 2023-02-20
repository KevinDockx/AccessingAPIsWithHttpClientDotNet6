namespace Movies.API.Models;

public class Trailer
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public byte[] Bytes { get; set; }

    public Trailer(Guid id,
       Guid movieId,
       string name,
       string? description,
       byte[] bytes)
    {
        Id = id;
        MovieId = movieId;
        Name = name;
        Description = description;
        Bytes = bytes;
    }

#pragma warning disable CS8618 
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Trailer()
#pragma warning restore CS8618  
    {

    }
}
