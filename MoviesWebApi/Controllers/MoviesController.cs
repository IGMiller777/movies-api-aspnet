using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.Models;
using MoviesWebApi.Database;
using MoviesWebApi.Utils;

namespace MoviesWebApi.Controllers
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
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }
            

            return await _dbContext.Movies.ToListAsync();
        }

        // GET api/Movies/id
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieResponse>> GetMovie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest(nameof(GetMovie));
            }

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!isMovieExists(id))
                {
                    return NotFound(nameof(GetMovie));
                }
                else
                {
                    throw;
                }

            }
            return NoContent();
        }

        //Delete api/Movie/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoveie(int id)
        {
            if (_dbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _dbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _dbContext.Remove(movie);
            await _dbContext.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool isMovieExists(long id)
        {
            return (_dbContext.Movies?.Any(x => x.Id == id)).GetValueOrDefault();
        }
    }
}
