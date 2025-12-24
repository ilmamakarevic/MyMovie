using Microsoft.EntityFrameworkCore;
using MyMovie.Application.DTOs;
using MyMovie.Domain.Entities;

namespace MyMovie.Infrastructure.Repositories
{
    public class MoviesDbContext : DbContext
    {
         public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

         public DbSet<MovieEntity> Movies { get; set; }
         public DbSet<ShowsEntity> TvShows { get; set; } 
         public DbSet<WatchlistItemEntity> WatchlistItems { get; set; }

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
            }
            );

            modelBuilder.Entity<ShowsEntity>(entity =>
{
    entity.HasKey(e => e.Id);
    
    entity.Property(e => e.Name) 
        .IsRequired()
        .HasMaxLength(200);
    
    entity.Property(e => e.Overview)
        .HasMaxLength(2000);
    
    entity.Property(e => e.PosterPath)
        .HasMaxLength(500);
    
    entity.Property(e => e.TmdbId)
        .IsRequired(false);

    entity.HasIndex(e => e.TmdbId);
});

modelBuilder.Entity<WatchlistItemEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.Title)
                    .HasMaxLength(200);
                
                entity.Property(e => e.Name)
                    .HasMaxLength(200);
                
                entity.Property(e => e.PosterPath)
                    .IsRequired()
                    .HasMaxLength(500);
                
                entity.Property(e => e.Overview)
                    .HasMaxLength(2000);
                
                entity.Property(e => e.AddedDate)
                    .IsRequired();

                entity.Property(e => e.Rating)
                    .IsRequired(false);

                // Index za brže pretrage i sprečavanje duplikata
                entity.HasIndex(e => new { e.TmdbId, e.Type, e.FirebaseUserId })
                    .IsUnique();
            });
        }
        
    }
}