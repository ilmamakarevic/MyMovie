using MyMovie.Application.DTOs;


namespace MyMovie.Application.Interfaces
{
    public interface IMovieExternalService
    {
        Task<MovieDto> GetMovieByIdAsync(int tmdbId);
        Task<List<MovieDto>> SearchMoviesAsync(string query);
        Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1);
    }
}
