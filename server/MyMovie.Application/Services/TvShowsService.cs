using Microsoft.Extensions.Logging;
using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;
using MyMovie.Domain.Entities;

namespace MyMovie.Application.Services
{
    public class TvShowsService : ITvShowService
    {
        private readonly ITvShowsRepository _repository;
        private readonly IMovieExternalService _tmdbService;
        private readonly ILogger<TvShowsService> _logger;

        public TvShowsService(
            ITvShowsRepository repository, 
            IMovieExternalService tmdbService,
            ILogger<TvShowsService> logger)
        {
            _repository = repository;
            _tmdbService = tmdbService;
            _logger = logger;
        }

        public async Task<List<ShowDto>> GetAllShowsAsync()
        {
            _logger.LogInformation("Dohvaćam sve serije iz baze podataka.");
            var shows = await _repository.GetAllAsync();
            return shows.Select(MapToDto).ToList();
        }

        public async Task<ShowDto?> GetShowByIdAsync(int id)
        {
            _logger.LogInformation("Dohvaćam seriju sa ID: {ShowId}", id);
            var show = await _repository.GetByIdAsync(id);
            
            if (show == null) return null;
            
            return MapToDto(show);
        }

        public async Task<bool> DeleteShowAsync(int id)
        {
            _logger.LogInformation("Brisanje serije sa ID: {ShowId}", id);
            await _repository.DeleteAsync(id);
            return await _repository.SaveAsync();
        }

        public async Task<List<ShowDto>> GetPopularShowsAsync(int page = 1)
        {
            _logger.LogInformation("Dohvaćam popularne serije sa TMDB, stranica: {Page}", page);
            return await _tmdbService.GetPopularTvShowsAsync(page);
        }

        public async Task<ShowDto> ImportShowFromTmdbAsync(int tmdbId)
        {
            _logger.LogInformation("Započinjem uvoz serije sa TMDB ID: {TmdbId}", tmdbId);

            // 1. Povlacenje podataka sa TMDB
            var showsFromTmdb = await _tmdbService.GetPopularTvShowsAsync(1);
            var showDto = showsFromTmdb.FirstOrDefault(s => s.Id == tmdbId);

            if (showDto == null)
            {
                _logger.LogError("Serija sa TMDB ID {TmdbId} nije pronađena.", tmdbId);
                throw new Exception("Show not found on TMDB");
            }

            // 2. Mapiranje u Entity 
            var entity = new ShowsEntity
            {
                Name = showDto.Name,
                Overview = showDto.Overview,
                PosterPath = showDto.PosterPath,
                VoteAverage = showDto.VoteAverage,
                FirstAirDate = showDto.FirstAirDate,
                TmdbId = showDto.Id,
                ImportedAt = DateTime.UtcNow
            };

            // 3. Spasavanje u bazu
            await _repository.AddAsync(entity);
            await _repository.SaveAsync();

            _logger.LogInformation("Serija '{Name}' uspješno uvezena sa lokalnim ID-em: {Id}", entity.Name, entity.Id);

            showDto.Id = entity.Id; // lokalni ID baze u DTO
            return showDto;
        }

        public async Task<List<ShowDto>> SearchTvShowsAsync(string query)
        {
            _logger.LogInformation("Searching tv shows on TMDB with query: {Query}", query);
            
            if (string.IsNullOrWhiteSpace(query))
            {
                // vraca praznu listu umjest exceptiona za prazan input
                return new List<ShowDto>(); 
            }

            var shows = await _tmdbService.SearchTvShowsAsync(query);
    
            _logger.LogInformation("Found {Count} shows for query: {Query}", shows.Count, query);
    
            return shows; 
        }


        private ShowDto MapToDto(ShowsEntity entity)
        {
            return new ShowDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Overview = entity.Overview,
                PosterPath = entity.PosterPath,
                VoteAverage = entity.VoteAverage,
                FirstAirDate = entity.FirstAirDate,
                TmdbId = entity.TmdbId
            };
        }

    }
}