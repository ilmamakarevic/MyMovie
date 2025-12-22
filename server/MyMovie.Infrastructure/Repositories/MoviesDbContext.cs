using Microsoft.EntityFrameworkCore;
using MyMovie.Application.DTOs;
using MyMovie.Domain.Entities;

namespace MyMovie.Infrastructure.Repositories
{
    public class MoviesDbContext : DbContext
    {
         public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

         public DbSet<MovieEntity> Movies { get; set; }

          protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(e => e.Overview)
                    .HasMaxLength(2000);
                
                entity.Property(e => e.PosterPath)
                    .HasMaxLength(500);
                
                entity.Property(e => e.TmdbId)
                    .IsRequired(false); // nullable
                
                entity.Property(e => e.ImportedAt)
                    .IsRequired(false); // nullable

                // Kreiranje indexa za brže pretrage po TMDB ID-u
                entity.HasIndex(e => e.TmdbId)
                    .IsUnique(false);
            });
        }
        
    }
}