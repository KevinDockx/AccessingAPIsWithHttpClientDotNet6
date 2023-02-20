using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Entities;
using Movies.API.Services;

namespace Movies.API.Controllers;

[Route("api/moviesstream")]
[ApiController]
public class MoviesStreamController : ControllerBase
{
    private readonly IMoviesRepository _moviesRepository;
    private readonly IMapper _mapper;

    public MoviesStreamController(IMoviesRepository moviesRepository,
        IMapper mapper)
    {
        _moviesRepository = moviesRepository ??
            throw new ArgumentNullException(nameof(moviesRepository));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async IAsyncEnumerable<Models.Movie> GetMoviesStream()
    {
        await foreach (var movie in
            _moviesRepository.GetMoviesAsAsyncEnumerable())
        {
            // for demo purposes, add a delay to visually see the effect
            await Task.Delay(500);
            yield return _mapper.Map<Models.Movie>(movie);
        } 
    } 
}
