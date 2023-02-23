using Microsoft.AspNetCore.Mvc;

namespace Movies.Client.Models;

public class ExtendedProblemDetailsWithErrors : ProblemDetails
{
    public Dictionary<string, string[]> Errors { get; set; }     
}
