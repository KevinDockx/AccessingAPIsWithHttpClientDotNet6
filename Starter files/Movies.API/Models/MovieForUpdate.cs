using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Movies.API.Models;

public class MovieForUpdate
{
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

    public MovieForUpdate(string title,
       DateTimeOffset releaseDate,
       Guid directorId,
       string? genre,
       string? description)
    {
        DirectorId = directorId;
        Title = title;
        Genre = genre;
        ReleaseDate = releaseDate;
        Description = description;
    }
#pragma warning disable CS8618 
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public MovieForUpdate()
#pragma warning restore CS8618  
    {

    }
}
