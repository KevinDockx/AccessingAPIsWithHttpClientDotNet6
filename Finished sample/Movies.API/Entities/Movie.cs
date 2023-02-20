using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.API.Entities;

[Table("Movies")]
public class Movie
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Genre { get; set; }

    [Required]
    public DateTimeOffset ReleaseDate { get; set; }

    [Required]
    public Guid DirectorId { get; set; }
    public Director Director { get; set; } = null!;

    public Movie(Guid id,
        Guid directorId,
        string title,
        DateTimeOffset releaseDate,
        string? genre,
        string? description)
    {
        Id = id;
        DirectorId = directorId;
        Title = title;
        ReleaseDate = releaseDate;
        Genre = genre;
        Description = description;
    }

#pragma warning disable CS8618
    // Parameterless constructor for easy AutoMapper integration
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Movie()
#pragma warning restore CS8618 
    {

    }
}
