using Microsoft.AspNetCore.Mvc;
using MyMovie.Application.DTOs;
using MyMovie.Application.Services;
using MyMovie.Application.Interfaces;

namespace MyMovie.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;


        public MoviesController(IMovieService movieService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _movieService = movieService;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Get all movies from database
        /// </summary>
        /// 

        [HttpGet]
        [ProducesResponseType(typeof(List<MovieDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MovieDto>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        /// <summary>
        /// Get movie by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            
            if (movie == null)
                return NotFound($"Movie with ID {id} not found");
            
            return Ok(movie);
        }

        /// <summary>
        /// Create a new movie manually
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDto>> CreateMovie([FromBody] MovieDto movieDto)
        {
            try
            {
                var created = await _movieService.CreateMovieAsync(movieDto);
                return CreatedAtAction(nameof(GetMovie), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update existing movie
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> UpdateMovie(int id, [FromBody] MovieDto movieDto)
        {
            try
            {
                var updated = await _movieService.UpdateMovieAsync(id, movieDto);
                
                if (updated == null)
                    return NotFound($"Movie with ID {id} not found");
                
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete movie by ID
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);
            
            if (!result)
                return NotFound($"Movie with ID {id} not found");
            
            return NoContent();
        }

        /// <summary>
        /// Search movies on TMDB
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<MovieDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MovieDto>>> SearchMovies([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty");

            var movies = await _movieService.SearchMoviesAsync(query);
            return Ok(movies);
        }

        /// <summary>
        /// Get popular movies from TMDB
        /// </summary>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(List<MovieDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MovieDto>>> GetPopularMovies([FromQuery] int page = 1)
        {
            var movies = await _movieService.GetPopularMoviesAsync(page);
            return Ok(movies);
        }
        
        /// <summary>
        /// Get movie details from TMDB (without saving)
        /// </summary>
        [HttpGet("tmdb/{tmdbId}")]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieDto>> GetMovieFromTmdb(int tmdbId)
        {
            try
            {
                var movie = await _movieService.GetMovieFromTmdbAsync(tmdbId);
                return Ok(movie);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Import movie from TMDB and save to database
        /// </summary>
        [HttpPost("import/{tmdbId}")]
        [ProducesResponseType(typeof(MovieDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MovieDto>> ImportFromTmdb(int tmdbId)
        {
            try
            {
                var movie = await _movieService.ImportMovieFromTmdbAsync(tmdbId);
                return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get total count of movies in database
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetMoviesCount()
        {
            var count = await _movieService.GetMoviesCountAsync();
            return Ok(count);
        }
    }
}