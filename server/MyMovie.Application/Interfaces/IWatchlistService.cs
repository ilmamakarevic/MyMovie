using MyMovie.Application.DTOs;

namespace MyMovie.Application.Interfaces
{
    public interface IWatchlistService
    {
        Task<List<WatchlistItemDto>> GetWatchlistAsync(string userId);
        Task<WatchlistItemDto> AddToWatchlistAsync(AddToWatchlistDto dto);
        Task<bool> RemoveFromWatchlistAsync(int id);
        Task<bool> IsOnWatchlistAsync(int tmdbId, string type, string userId);
    }
}