using Microsoft.EntityFrameworkCore;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;

namespace MovieManagement.DataAccess.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Movie> GetAll()
        {
            return _dbSet
                .Include(m => m.Actor)
                .Include(m => m.Genre)
                .ToList();
        }

        public override Movie? GetById(int id)
        {
            return _dbSet
                .Include(m => m.Actor)
                .Include(m => m.Genre)
                .FirstOrDefault(m => m.Id == id);
        }
    }
}
