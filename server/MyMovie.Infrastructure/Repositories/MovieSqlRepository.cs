using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMovie.Application.Interfaces;
using MyMovie.Domain.Entities;

namespace MyMovie.Infrastructure.Repositories
{
    public class MovieSqlRepository : IMovieRepository
    {
        private readonly MoviesDbContext _moviesDbContext;
        public MovieSqlRepository(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        public async Task AddAsync(MovieEntity item)
        {
            await _moviesDbContext.Movies.AddAsync(item);
        }

        public async Task<int> CountAsync()
        {
            return await _moviesDbContext.Movies.CountAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await GetByIdAsync(id);
            if (movie != null)
            {
                _moviesDbContext.Movies.Remove(movie);
            }
        }

        public async Task<List<MovieEntity>> GetAllAsync()
        {
            return await _moviesDbContext.Movies.ToListAsync();
        }

        public async Task<MovieEntity> GetByIdAsync(int id)
        {
            return await _moviesDbContext.Movies.FindAsync(id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _moviesDbContext.SaveChangesAsync() > 0;
        }

        public async Task<MovieEntity> UpdateAsync(int id, MovieEntity item)
        {
            var existingMovie = await GetByIdAsync(id);
            if (existingMovie != null)
            {
                existingMovie.Title = item.Title;
                existingMovie.Overview = item.Overview;
                existingMovie.PosterPath = item.PosterPath;
                existingMovie.ReleaseDate = item.ReleaseDate;
                existingMovie.VoteAverage = item.VoteAverage;
                
                _moviesDbContext.Movies.Update(existingMovie);
            }
            return existingMovie;
        }
    }
}