using Microsoft.AspNetCore.Mvc;
using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;

namespace MyMovie.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        /// <summary>
        /// Dohvati sve stavke s watchliste
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<WatchlistItemDto>>> GetWatchlist()
        {
            var items = await _watchlistService.GetWatchlistAsync();
            
            if (items == null || !items.Any())
                return NoContent();
                
            return Ok(items);
        }

        /// <summary>
        /// Dodaj film/seriju na watchlist
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<WatchlistItemDto>> AddToWatchlist([FromBody] AddToWatchlistDto dto)
        {
            try
            {
                var item = await _watchlistService.AddToWatchlistAsync(dto);
                return Ok(item);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Ukloni stavku s watchliste
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            var result = await _watchlistService.RemoveFromWatchlistAsync(id);
            
            if (!result)
                return NotFound($"Watchlist item with ID {id} not found");
                
            return NoContent();
        }

        /// <summary>
        /// Provjeri da li je film/serija na watchlisti
        /// </summary>
        [HttpGet("check/{tmdbId}")]
        public async Task<ActionResult<bool>> IsOnWatchlist(int tmdbId, [FromQuery] string type)
        {
            var isOnList = await _watchlistService.IsOnWatchlistAsync(tmdbId, type);
            return Ok(isOnList);
        }
    }
}