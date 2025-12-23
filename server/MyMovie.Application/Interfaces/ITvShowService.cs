using MyMovie.Application.DTOs;

namespace MyMovie.Application.Interfaces
{
    public interface ITvShowService
    {
        // CRUD za bazu podataka
        Task<List<ShowDto>> GetAllShowsAsync();
        Task<ShowDto?> GetShowByIdAsync(int id);
        Task<bool> DeleteShowAsync(int id);

        // TMDB integracija
        Task<List<ShowDto>> GetPopularShowsAsync(int page = 1);
        Task<ShowDto> ImportShowFromTmdbAsync(int tmdbId);
    }
}