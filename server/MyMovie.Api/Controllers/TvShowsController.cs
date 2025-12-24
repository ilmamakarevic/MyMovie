using Microsoft.AspNetCore.Mvc;
using MyMovie.Application.DTOs;
using MyMovie.Application.Interfaces;

namespace MyMovie.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TvShowsController : ControllerBase
    {
        private readonly ITvShowService _tvShowService;

        public TvShowsController(ITvShowService tvShowService)
        {
            _tvShowService = tvShowService;
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<ShowDto>>> GetPopular()
        {
            var shows = await _tvShowService.GetPopularShowsAsync();
            return Ok(shows);
        }

        [HttpGet]
        public async Task<ActionResult<List<ShowDto>>> GetAllFromDb()
        {
            var shows = await _tvShowService.GetAllShowsAsync();
            return Ok(shows);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tvShowService.DeleteShowAsync(id);
            return NoContent();
        }

        
    }
}