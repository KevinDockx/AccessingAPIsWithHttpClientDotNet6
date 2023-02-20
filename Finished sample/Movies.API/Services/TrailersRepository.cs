using Microsoft.EntityFrameworkCore;
using Movies.API.DbContexts;
using Movies.API.InternalModels;

namespace Movies.API.Services;

public class TrailersRepository : ITrailersRepository 
{
    private MoviesDbContext _context;

    public TrailersRepository(MoviesDbContext context)
    {
        _context = context ?? 
            throw new ArgumentNullException(nameof(context));

    }

    public async Task<Trailer?> GetTrailerAsync(Guid movieId, Guid trailerId)
    {
        // Generate the name from the movie title.
        var movie = await _context.Movies
         .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
        {
            throw new Exception($"Movie with id {movieId} not found.");
        }

        // generate a trailer (byte array) between 50 and 100MB
        var random = new Random();
        var generatedByteLength = random.Next(52428800, 104857600);
        var generatedBytes = new byte[generatedByteLength];
        random.NextBytes(generatedBytes);

        return new Trailer(
         trailerId,
         movieId, 
         $"{movie.Title} trailer number {DateTime.UtcNow.Ticks}",
         generatedBytes,
         $"{movie.Title} trailer description {DateTime.UtcNow.Ticks}");
    }

    public async Task<Trailer> AddTrailer(Guid movieId, Trailer trailerToAdd)
    {
        // don't do anything: we're just faking this.  Simply return the trailer
        // after setting the ids
        trailerToAdd.MovieId = movieId;
        trailerToAdd.Id = Guid.NewGuid();
        return await Task.FromResult(trailerToAdd);
    } 
}
