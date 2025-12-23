namespace MyMovie.Application.DTOs
{
    public class ShowDto
    {
        public int Id { get; set; }
        public string? Name{ get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public double VoteAverage { get; set; }
        public DateTime? FirstAirDate { get; set; }
        public int? TmdbId { get; set; }    
    }
}