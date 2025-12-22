using Microsoft.EntityFrameworkCore;
using MyMovie.Application.DTOs;
using MyMovie.Domain.Entities;

namespace MyMovie.Infrastructure.Repositories
{
    public class MoviesDbContext : DbContext
    {
         public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

         public DbSet<MovieEntity> Movies { get; set; }
        
    }
}