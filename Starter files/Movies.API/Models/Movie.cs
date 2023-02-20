namespace Movies.API.Models;

public class Movie
{     
    public Guid Id { get; set; } 
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public DateTimeOffset ReleaseDate { get; set; }
    public string Director { get; set; }

    public Movie(Guid id, 
        string title,
        DateTimeOffset releaseDate,
        string? genre,
        string? description,
        string director)
    {
        Id = id;
        Director = director;
        Title = title;
        Genre = genre;
        ReleaseDate = releaseDate;
        Description = description;
    }

#pragma warning disable CS8618
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Movie()
#pragma warning restore CS8618 
    {

    }
     
}
