using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;

namespace MovieManagement.DataAccess.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
