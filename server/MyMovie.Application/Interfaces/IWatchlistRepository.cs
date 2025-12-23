using MyMovie.Domain.Entities;

namespace MyMovie.Application.Interfaces
{
    public interface IWatchlistRepository
    {
        Task<List<WatchlistItemEntity>> GetAllAsync();
        Task<WatchlistItemEntity> AddAsync(WatchlistItemEntity item);
        Task<bool> RemoveAsync(int id);
        Task<bool> ExistsAsync(int tmdbId, string type);
    }
}