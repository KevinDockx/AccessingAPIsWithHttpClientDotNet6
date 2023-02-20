using System.ComponentModel.DataAnnotations;

namespace Movies.API.InternalModels;

public class Trailer
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid MovieId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public byte[] Bytes { get; set; }

    public Trailer(Guid id, 
        Guid movieId, 
        string name, 
        byte[] bytes, 
        string? description)
    {
        Id = id;
        MovieId = MovieId;
        Name = name;
        Bytes = bytes;
        Description = description; 
    }

#pragma warning disable CS8618
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Trailer()
#pragma warning restore CS8618 
    {

    }

}
