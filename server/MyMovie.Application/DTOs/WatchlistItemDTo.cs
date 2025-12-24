// WatchlistItemDto.cs
namespace MyMovie.Application.DTOs
{
    public class WatchlistItemDto
    {
        public int Id { get; set; }
        public int TmdbId { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? PosterPath { get; set; }
        public string? Type { get; set; } // "movie" ili "tv"
        public DateTime AddedDate { get; set; }
        public double? Rating { get; set; }
        public string? Overview { get; set; }
    }
}

// AddToWatchlistDto.cs
namespace MyMovie.Application.DTOs
{
    public class AddToWatchlistDto
    {
        public int TmdbId { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? PosterPath { get; set; }
        public string? Type { get; set; } // "movie" ili "tv"
        public double? Rating { get; set; }
        public string? Overview { get; set; }
    }
}