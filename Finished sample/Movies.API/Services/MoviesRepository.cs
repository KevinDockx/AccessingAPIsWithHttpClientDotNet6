using Microsoft.EntityFrameworkCore;
using Movies.API.DbContexts;
using Movies.API.Entities; 

namespace Movies.API.Services;

public class MoviesRepository : IMoviesRepository 
{
    private MoviesDbContext _context;

    public MoviesRepository(MoviesDbContext context)
    {
        _context = context ?? 
            throw new ArgumentNullException(nameof(context));
    }

    public async Task<Movie?> GetMovieAsync(Guid movieId)
    {
        return await _context.Movies.Include(m => m.Director)
            .FirstOrDefaultAsync(m => m.Id == movieId);
    } 

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        return await _context.Movies
            .Include(m => m.Director).ToListAsync();
    }

    public IAsyncEnumerable<Movie> GetMoviesAsAsyncEnumerable()
    {
        return _context.Movies.AsAsyncEnumerable<Movie>();
    }

    public void UpdateMovie(Movie movieToUpdate)
    {
        // no code required, entity tracked by context.  Including 
        // this is best practice to ensure other implementations of the 
        // contract (eg a mock version) can execute code on update
        // when needed.
    } 

    public void AddMovie(Movie movieToAdd)
    {
        if (movieToAdd == null)
        {
            throw new ArgumentNullException(nameof(movieToAdd));
        }

        _context.Add(movieToAdd);
    }

    public void DeleteMovie(Movie movieToDelete)
    {
        if (movieToDelete == null)
        {
            throw new ArgumentNullException(nameof(movieToDelete));
        }

        _context.Remove(movieToDelete);
    }

    public async Task<bool> SaveChangesAsync()
    {
        // return true if 1 or more entities were changed
        return (await _context.SaveChangesAsync() > 0);
    } 
}
