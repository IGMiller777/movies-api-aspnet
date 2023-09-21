using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.Models;
using MoviesWebApi.Database;
using MoviesWebApi.Utils;

namespace MoviesWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _dbContext;

        public MoviesController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieResponse>>> GetMovies()
        {
            var movies = await _dbContext.Movies.ToListAsync();
            if (movies == null || movies.Count == 1)
            {
                return NotFound();
            }

            return Ok(Mapper.Convert(movies));
        }

        // GET api/Movies/id
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            
            return Mapper.Convert(movie);
        }

        // POST Movie
        [HttpPost]
        public async Task<ActionResult<MovieResponse>> PostMovie(MovieRequest movieRequest)
        {
            try
            {
                var movie = Mapper.Convert(movieRequest);
                _dbContext.Movies.Add(movie);
                await _dbContext.SaveChangesAsync();
                
                return Mapper.Convert(movie);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Put api/Movie/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieRequest movie)
        {
            var existedMovie = await _dbContext.Movies.FindAsync(id);
            
            if (existedMovie == null)
            {
                return NotFound($"Movie with ID: {id} not found!");
            }

            existedMovie.Title = movie.Title;
            existedMovie.Genre = movie.Genre;
            existedMovie.ReleaseDate = movie.ReleaseDate;
            // _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!isMovieExists(id))
                {
                    return NotFound($"Movie with ID: {id} not found!");
                }
            }
            return NoContent();
        }

        //Delete api/Movie/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existedMovie = await _dbContext.Movies.FindAsync(id);

            if (existedMovie == null)
            {
                return NotFound($"Movie with ID: {id} not found!");
            }

            _dbContext.Remove(existedMovie);
            await _dbContext.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool isMovieExists(long id)
        {
            return (_dbContext.Movies?.Any(x => x.Id == id)).GetValueOrDefault();
        }
    }
}
