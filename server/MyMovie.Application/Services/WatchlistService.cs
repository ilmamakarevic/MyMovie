using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;
using MyMovie.Domain.Entities;

namespace MyMovie.Application.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _repository;

        public WatchlistService(IWatchlistRepository repository)
        {
            _repository = repository;
        }

        // 1. Dodaj parametar userId
        public async Task<List<WatchlistItemDto>> GetWatchlistAsync(string userId)
        {
            // Pozivamo novu metodu koju smo dodali u Repository
            var items = await _repository.GetAllByUserIdAsync(userId); 
            return items.Select(MapToDto).ToList();
        }

        public async Task<WatchlistItemDto> AddToWatchlistAsync(AddToWatchlistDto dto)
        {
            // 2. Proslijedi FirebaseUserId iz DTO-a u provjeru postojanja
            var exists = await _repository.ExistsAsync(dto.TmdbId, dto.Type, dto.FirebaseUserId);
            if (exists)
                throw new InvalidOperationException("Item already exists on your watchlist");

            var item = new WatchlistItemEntity
            {
                TmdbId = dto.TmdbId,
                FirebaseUserId = dto.FirebaseUserId, // 3. OBAVEZNO spremi ID korisnika u Entity
                Name = dto.Type == "tv" ? dto.Name : null,
                Title = dto.Type == "movie" ? dto.Title : null,
                PosterPath = dto.PosterPath,
                Type = dto.Type,
                Rating = dto.Rating,
                Overview = dto.Overview,
                AddedDate = DateTime.UtcNow
            };

            var added = await _repository.AddAsync(item);
            return MapToDto(added);
        }

        public async Task<bool> RemoveFromWatchlistAsync(int id)
        {
            return await _repository.RemoveAsync(id);
        }

        // 4. Dodaj parametar userId
        public async Task<bool> IsOnWatchlistAsync(int tmdbId, string type, string userId)
        {
            // Provjeri postojanje za točno tog korisnika
            return await _repository.ExistsAsync(tmdbId, type, userId);
        }

        private WatchlistItemDto MapToDto(WatchlistItemEntity item)
        {
            return new WatchlistItemDto
            {
                Id = item.Id,
                TmdbId = item.TmdbId,
                Title = item.Title,
                Name = item.Name,
                PosterPath = item.PosterPath,
                Type = item.Type,
                Rating = item.Rating,
                Overview = item.Overview,
                AddedDate = item.AddedDate
            };
        }
    }
}