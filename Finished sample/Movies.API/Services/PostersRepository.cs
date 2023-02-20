using Microsoft.EntityFrameworkCore;
using Movies.API.DbContexts;
using Movies.API.InternalModels;

namespace Movies.API.Services;

public class PostersRepository : IPostersRepository 
{
    private readonly MoviesDbContext _context;

    public PostersRepository(MoviesDbContext context)
    {
        _context = context ?? 
            throw new ArgumentNullException(nameof(context));
    }

    public async Task<Poster?> GetPosterAsync(Guid movieId, Guid posterId)
    {
        // Generate the name from the movie title.
        var movie = await _context.Movies
         .FirstOrDefaultAsync(m => m.Id == movieId);

        if (movie == null)
        {
            throw new Exception($"Movie with id {movieId} not found.");
        }

        // generate a movie poster of 500KB
        var random = new Random();
        var generatedBytes = new byte[524288];
        random.NextBytes(generatedBytes);

        return new Poster(posterId,
            movieId,
            $"{movie.Title} poster number {DateTime.UtcNow.Ticks}",
            generatedBytes); 
    }

    public async Task<Poster> AddPoster(Guid movieId, Poster posterToAdd)
    {
        // don't do anything: we're just faking this.  Simply return the poster
        // after setting the ids
        posterToAdd.MovieId = movieId;
        posterToAdd.Id = Guid.NewGuid();
        return await Task.FromResult(posterToAdd);
    } 
}
