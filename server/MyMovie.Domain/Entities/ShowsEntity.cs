using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMovie.Domain.Entities
{
    public class ShowsEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Overview { get; set; }
        public string? PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double VoteAverage { get; set; }
        public int? TmdbId { get; set; }           
        public DateTime? FirstAirDate { get; set; }
      
        public DateTime? ImportedAt { get; set; }
    }
    

}