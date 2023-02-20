using Movies.API.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Movies.API.Models;

public class PosterForCreation
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Required]
    public byte[] Bytes { get; set; }

    public PosterForCreation( 
      string name,
      byte[] bytes)
    { 
        Name = name;
        Bytes = bytes;
    }

#pragma warning disable CS8618 
    // Parameterless constructor required for XML serialization
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PosterForCreation()
#pragma warning restore CS8618  
    {

    }
}
