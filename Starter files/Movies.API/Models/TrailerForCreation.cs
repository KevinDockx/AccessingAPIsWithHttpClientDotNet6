using System.ComponentModel.DataAnnotations;

namespace Movies.API.Models;

public class TrailerForCreation
{
    [Required]
    public Guid MovieId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public byte[] Bytes { get; set; }

    public TrailerForCreation(Guid movieId,
        string name,
        string? description,
        byte[] bytes)
    {
        MovieId = movieId;
        Name = name;
        Description = description;
        Bytes = bytes;
    }

#pragma warning disable CS8618 
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TrailerForCreation()
#pragma warning restore CS8618  
    {

    }
}
