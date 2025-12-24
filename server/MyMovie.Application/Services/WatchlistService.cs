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

        public async Task<List<WatchlistItemDto>> GetWatchlistAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<WatchlistItemDto> AddToWatchlistAsync(AddToWatchlistDto dto)
        {
            var exists = await _repository.ExistsAsync(dto.TmdbId, dto.Type);
            if (exists)
                throw new InvalidOperationException("Item already exists on watchlist");

            var item = new WatchlistItemEntity
            {
                TmdbId = dto.TmdbId,
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

        public async Task<bool> IsOnWatchlistAsync(int tmdbId, string type)
        {
            return await _repository.ExistsAsync(tmdbId, type);
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