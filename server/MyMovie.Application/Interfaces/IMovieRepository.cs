using MyMovie.Domain.Entities;

namespace MyMovie.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<MovieEntity> GetByIdAsync(int id);
        Task<List<MovieEntity>> GetAllAsync();
        Task AddAsync(MovieEntity item);
        Task DeleteAsync(int id);
        Task<MovieEntity> UpdateAsync(int id, MovieEntity item);
        Task<int> CountAsync();
        Task<bool> SaveAsync();
    }
}

