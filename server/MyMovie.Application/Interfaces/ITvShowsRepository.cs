using MyMovie.Domain.Entities;

namespace MyMovie.Application.Interfaces
{
    
    public interface ITvShowsRepository
    {
        Task<ShowsEntity> GetByIdAsync(int id);
        Task<List<ShowsEntity>> GetAllAsync();
        Task AddAsync(ShowsEntity item);
        Task DeleteAsync(int id);
        Task<ShowsEntity> UpdateAsync(int id, ShowsEntity item);
        Task<int> CountAsync();
        Task<bool> SaveAsync();
    }
}

