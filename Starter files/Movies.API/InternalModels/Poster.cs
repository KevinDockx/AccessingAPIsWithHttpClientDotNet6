using System.ComponentModel.DataAnnotations;

namespace Movies.API.InternalModels;

public class Poster
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid MovieId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    public byte[] Bytes { get; set; }

    public Poster(Guid id, 
        Guid movieId,
        string name, 
        byte[] bytes)
    {
        Id = id;
        MovieId = movieId;
        Name = name;
        Bytes = bytes;
    }

#pragma warning disable CS8618
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Poster()
#pragma warning restore CS8618 
    {

    }
}
