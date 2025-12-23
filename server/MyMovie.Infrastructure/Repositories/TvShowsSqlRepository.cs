using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMovie.Application.Interfaces;
using MyMovie.Domain.Entities;

namespace MyMovie.Infrastructure.Repositories
{
    public class TvShowsSqlRepository : ITvShowsRepository
    {
        private readonly MoviesDbContext _moviesDbContext;

        public TvShowsSqlRepository(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }
        public async Task AddAsync(ShowsEntity item)
        {
            await _moviesDbContext.TvShows.AddAsync(item);
        }

        public async Task<int> CountAsync()
        {
            return await _moviesDbContext.TvShows.CountAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var show = await GetByIdAsync(id);
            if (show != null)
            {
                _moviesDbContext.TvShows.Remove(show);
            }
        }

        public async Task<List<ShowsEntity>> GetAllAsync()
        {
            return await _moviesDbContext.TvShows.ToListAsync();
        }

        public async Task<ShowsEntity> GetByIdAsync(int id)
        {
            return await _moviesDbContext.TvShows.FindAsync(id);
        }

        public async Task<bool> SaveAsync()
        {
            // Vraća true ako je barem jedan red u bazi promijenjen
            return await _moviesDbContext.SaveChangesAsync() > 0;
        }

        public async Task<ShowsEntity> UpdateAsync(int id, ShowsEntity item)
        {
            var existingShow = await GetByIdAsync(id);
            if (existingShow != null)
            {
                existingShow.Name = item.Name;
                existingShow.Overview = item.Overview;
                existingShow.PosterPath = item.PosterPath;
                existingShow.VoteAverage = item.VoteAverage;
                existingShow.FirstAirDate = item.FirstAirDate;
                
                _moviesDbContext.TvShows.Update(existingShow);
            }
            return existingShow;
        }
    }
}