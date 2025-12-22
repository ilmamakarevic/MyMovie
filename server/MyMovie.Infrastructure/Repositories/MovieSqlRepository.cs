using System.Threading.Tasks;
using MyMovie.Application;
using MyMovie.Domain.Entities;
using MyMovie.Domain.Interfaces;

namespace MyMovie.Infrastructure.Repositories
{
    public class MovieSqlRepository : IMovieRepository
    {
        private readonly MoviesDbContext _moviesDbContext;

        public MovieSqlRepository(MoviesDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        public MovieEntity GetSingleById(int id)
        {
          return _moviesDbContext.Movies.FirstOrDefault(x => x.Id == id);   
        }

        public void Add(MovieEntity item)
        {
            _moviesDbContext.Movies.Add(item);
        }
        public MovieEntity Update(int id, MovieEntity item)
        {
            _moviesDbContext.Movies.Update(item);
            return item;
        }

        public void Delete(int id)
        {
            MovieEntity movieItem = GetSingleById(id);
            _moviesDbContext.Movies.Remove(movieItem);

        }

        // public ICollection<MovieEntity> GetRandomMovie()
        // {
        //     
        // }

        public int Count()
        {
            return _moviesDbContext.Movies.Count();
        }
        public bool Save()
        {
            return _moviesDbContext.SaveChanges()>=0;
        }

        
    }
}