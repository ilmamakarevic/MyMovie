using MyMovie.Domain.Entities;

namespace MyMovie.Domain.Interfaces
{
    public interface IMovieRepository
    {
        MovieEntity GetSingleById(int id);
        // IQueryable<MovieEntity> GetAll(QueryParameters queryParameters);
        void Add(MovieEntity item);
        void Delete(int id);
        MovieEntity Update(int id, MovieEntity item);
        // ICollection<MovieEntity> GetRandomMovie(); //napraviti metodu
        int Count();
        bool Save();
    }
}

