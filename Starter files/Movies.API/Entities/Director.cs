using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.API.Entities;

[Table("Directors")]
public class Director
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(200)]
    public string LastName { get; set; }

    public Director(Guid id, 
        string firstName, 
        string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

#pragma warning disable CS8618
    // Parameterless constructor for easy AutoMapper integration
    // Disabled warning: Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Director()
#pragma warning restore CS8618 
    {

    }
}
