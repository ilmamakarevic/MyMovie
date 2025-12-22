using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMovie.Domain.Entities
{
    public class MovieEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
    

}