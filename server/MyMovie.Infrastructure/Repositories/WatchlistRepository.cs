using MyMovie.Domain.Entities;
using MyMovie.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MyMovie.Infrastructure.Repositories
{
    public class WatchlistRepository : IWatchlistRepository
    {
        private readonly MoviesDbContext _context;

        public WatchlistRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<List<WatchlistItemEntity>> GetAllAsync()
        {
            return await _context.WatchlistItems
                .OrderByDescending(x => x.AddedDate)
                .ToListAsync();
        }

        public async Task<List<WatchlistItemEntity>> GetAllByUserIdAsync(string userId)
        {
            return await _context.WatchlistItems.Where(x => x.FirebaseUserId == userId).ToListAsync();
        }

        public async Task<WatchlistItemEntity> AddAsync(WatchlistItemEntity item)
        {
            _context.WatchlistItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await _context.WatchlistItems.FindAsync(id);
            if (item == null) return false;

            _context.WatchlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int tmdbId, string type, string userId)
        {
            return await _context.WatchlistItems.AnyAsync(x => x.TmdbId == tmdbId && x.Type == type && x.FirebaseUserId == userId);
        }


    }
}