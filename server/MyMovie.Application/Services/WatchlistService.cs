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

        public async Task<List<WatchlistItemDto>> GetWatchlistAsync(string userId)
        {
            var items = await _repository.GetAllByUserIdAsync(userId); 
            return items.Select(MapToDto).ToList();
        }

        public async Task<WatchlistItemDto> AddToWatchlistAsync(AddToWatchlistDto dto)
        {
            // proslijedjuje FirebaseUserId iz DTO-a u provjeru postojanja
            var exists = await _repository.ExistsAsync(dto.TmdbId, dto.Type, dto.FirebaseUserId);
            if (exists)
                throw new InvalidOperationException("Item already exists on your watchlist");

            var item = new WatchlistItemEntity
            {
                TmdbId = dto.TmdbId,
                FirebaseUserId = dto.FirebaseUserId, //sprema ID korisnika u entity
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

        public async Task<bool> IsOnWatchlistAsync(int tmdbId, string type, string userId)
        {
            // Provjeri postojanje za tacno tog korisnika
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