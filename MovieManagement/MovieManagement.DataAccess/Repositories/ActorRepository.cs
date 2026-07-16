using Microsoft.EntityFrameworkCore;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;

namespace MovieManagement.DataAccess.Repositories
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        public ActorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IEnumerable<Actor> GetAll()
        {
            return _dbSet
                .Include(a => a.Biography)
                .Include(a => a.Movies)
                .ToList();
        }
    }
}
