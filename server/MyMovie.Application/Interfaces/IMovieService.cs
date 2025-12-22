using MyMovie.Application.DTOs;

namespace MyMovie.Application.Interfaces
{
    public interface IMovieService
    {
         Task<List<MovieDto>> GetAllMoviesAsync();
        Task<MovieDto?> GetMovieByIdAsync(int id);
        Task<MovieDto> CreateMovieAsync(MovieDto movieDto);
        Task<MovieDto?> UpdateMovieAsync(int id, MovieDto movieDto);
        Task<bool> DeleteMovieAsync(int id);
        
        // TMDB integracija
        Task<List<MovieDto>> SearchMoviesAsync(string query);
        Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1);
        Task<MovieDto> GetMovieFromTmdbAsync(int tmdbId);
        Task<MovieDto> ImportMovieFromTmdbAsync(int tmdbId);
        
        // Dodatne operacije
        Task<int> GetMoviesCountAsync();
    }
}