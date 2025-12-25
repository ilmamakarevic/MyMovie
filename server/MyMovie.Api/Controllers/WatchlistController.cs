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

        /// Dohvati sve sa watchlist
        [HttpGet]
        public async Task<ActionResult<List<WatchlistItemDto>>> GetWatchlist([FromQuery] string userId)
        {
            var items = await _watchlistService.GetWatchlistAsync(userId);
            
            if (items == null || !items.Any())
                return NoContent();
                
            return Ok(items);
        }

        /// Dodaj film/seriju na watchlist
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

        
        /// Ukloni stavku s watchliste
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            var result = await _watchlistService.RemoveFromWatchlistAsync(id);
            
            if (!result)
                return NotFound($"Watchlist item with ID {id} not found");
                
            return NoContent();
        }

        /// Provjeri da li je film/serija na watchlisti
        [HttpGet("check/{tmdbId}")]
        public async Task<ActionResult<bool>> IsOnWatchlist(int tmdbId, [FromQuery] string type, [FromQuery] string userId)
        {
            var items = await _watchlistService.IsOnWatchlistAsync(tmdbId, type, userId);

            return Ok(items);
        }
    }
    
}