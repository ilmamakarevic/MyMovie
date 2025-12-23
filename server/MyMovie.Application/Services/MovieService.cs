using Microsoft.Extensions.Logging;
using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;
using MyMovie.Domain.Entities;

namespace MyMovie.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IMovieExternalService _tmdbService;
        private readonly ILogger<MovieService> _logger;

        public MovieService(
            IMovieRepository repository,
            IMovieExternalService tmdbService,
            ILogger<MovieService> logger)
        {
            _repository = repository;
            _tmdbService = tmdbService;
            _logger = logger;
        }

        #region CRUD Operations

        public async Task<List<MovieDto>> GetAllMoviesAsync()
        {
            _logger.LogInformation("Getting all movies from database");
            
            var movies = await _repository.GetAllAsync();
            return movies.Select(MapToDto).ToList();
        }

        public async Task<MovieDto?> GetMovieByIdAsync(int id)
        {
            _logger.LogInformation("Getting movie with ID: {MovieId}", id);
            
            var movie = await _repository.GetByIdAsync(id);
            
            if (movie == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found", id);
                return null;
            }
            
            return MapToDto(movie);
        }

        public async Task<MovieDto> CreateMovieAsync(MovieDto movieDto)
        {
            _logger.LogInformation("Creating new movie: {MovieTitle}", movieDto.Title);
            
            // Validacija
            if (string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be empty");
            }

            if (movieDto.ReleaseDate > DateTime.Now)
            {
                throw new ArgumentException("Release date cannot be in the future");
            }

            // Mapiranje i dodavanje
            var entity = MapToEntity(movieDto);
            await _repository.AddAsync(entity);
            
            var saved = await _repository.SaveAsync();
            if (!saved)
            {
                throw new InvalidOperationException("Failed to save movie to database");
            }

            _logger.LogInformation("Movie created successfully with ID: {MovieId}", entity.Id);
            
            movieDto.Id = entity.Id;
            return movieDto;
        }

        public async Task<MovieDto?> UpdateMovieAsync(int id, MovieDto movieDto)
        {
            _logger.LogInformation("Updating movie with ID: {MovieId}", id);
            
            var existingMovie = await _repository.GetByIdAsync(id);
            if (existingMovie == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found for update", id);
                return null;
            }

            // Validacija
            if (string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be empty");
            }

            // Ažuriranje
            var updatedEntity = await _repository.UpdateAsync(id, MapToEntity(movieDto));
            
            var saved = await _repository.SaveAsync();
            if (!saved)
            {
                throw new InvalidOperationException("Failed to update movie");
            }

            _logger.LogInformation("Movie with ID {MovieId} updated successfully", id);
            
            return MapToDto(updatedEntity);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            _logger.LogInformation("Deleting movie with ID: {MovieId}", id);
            
            var movie = await _repository.GetByIdAsync(id);
            if (movie == null)
            {
                _logger.LogWarning("Movie with ID {MovieId} not found for deletion", id);
                return false;
            }

            await _repository.DeleteAsync(id);
            var saved = await _repository.SaveAsync();

            if (saved)
            {
                _logger.LogInformation("Movie with ID {MovieId} deleted successfully", id);
            }
            
            return saved;
        }

        #endregion

        #region TMDB Integration

        public async Task<List<MovieDto>> SearchMoviesAsync(string query)
        {
            _logger.LogInformation("Searching movies on TMDB with query: {Query}", query);
            
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Search query cannot be empty");
            }

            var movies = await _tmdbService.SearchMoviesAsync(query);
            
            _logger.LogInformation("Found {Count} movies for query: {Query}", movies.Count, query);
            
            return movies;
        }

        public async Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1)
        {
            _logger.LogInformation("Getting popular movies from TMDB, page: {Page}", page);
            
            if (page < 1)
            {
                throw new ArgumentException("Page number must be greater than 0");
            }

            var movies = await _tmdbService.GetPopularMoviesAsync(page);
            
            _logger.LogInformation("Retrieved {Count} popular movies", movies.Count);
            
            return movies;
        }

        public async Task<MovieDto> GetMovieFromTmdbAsync(int tmdbId)
        {
            _logger.LogInformation("Getting movie from TMDB with ID: {TmdbId}", tmdbId);
            
            var movie = await _tmdbService.GetMovieByIdAsync(tmdbId);
            
            if (movie == null)
            {
                throw new InvalidOperationException($"Movie with TMDB ID {tmdbId} not found");
            }
            
            return movie;
        }

        public async Task<MovieDto> ImportMovieFromTmdbAsync(int tmdbId)
        {
            _logger.LogInformation("Importing movie from TMDB with ID: {TmdbId}", tmdbId);
            
            // Provjerava da li film već postoji u bazi
            var existingMovies = await _repository.GetAllAsync();
            var alreadyExists = existingMovies.Any(m => m.TmdbId == tmdbId);
            
            if (alreadyExists)
            {
                _logger.LogWarning("Movie with TMDB ID {TmdbId} already exists in database", tmdbId);
                throw new InvalidOperationException($"Movie with TMDB ID {tmdbId} is already imported");
            }

            // Preuzima film sa TMDB
            var tmdbMovie = await _tmdbService.GetMovieByIdAsync(tmdbId);
            
            if (tmdbMovie == null)
            {
                throw new InvalidOperationException($"Movie with TMDB ID {tmdbId} not found on TMDB");
            }

            // Kreiraj entitet i dodaj TMDB ID
            var entity = MapToEntity(tmdbMovie);
            entity.TmdbId = tmdbId;
            entity.ImportedAt = DateTime.UtcNow;

            // Sačuvaj u bazu
            await _repository.AddAsync(entity);
            var saved = await _repository.SaveAsync();
            
            if (!saved)
            {
                throw new InvalidOperationException("Failed to import movie to database");
            }

            _logger.LogInformation("Movie '{Title}' imported successfully with ID: {MovieId}", 
                entity.Title, entity.Id);
            
            tmdbMovie.Id = entity.Id;
            return tmdbMovie;
        }

        #endregion

        #region Additional Operations

        public async Task<int> GetMoviesCountAsync()
        {
            _logger.LogInformation("Getting total movies count");
            
            var count = await _repository.CountAsync();
            
            _logger.LogInformation("Total movies in database: {Count}", count);
            
            return count;
        }

        #endregion

        #region Private Mapping Methods

        private MovieDto MapToDto(MovieEntity entity)
        {
            return new MovieDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Overview = entity.Overview,
                PosterPath = entity.PosterPath,
                ReleaseDate = entity.ReleaseDate,
                VoteAverage = entity.VoteAverage
            };
        }
        private MovieEntity MapToEntity(MovieDto dto)
        {
            return new MovieEntity
            {
                Id = dto.Id,
                Title = dto.Title,
                Overview = dto.Overview,
                PosterPath = dto.PosterPath,
                ReleaseDate = dto.ReleaseDate,
                VoteAverage = dto.VoteAverage
            };
        }

        #endregion
    }
}