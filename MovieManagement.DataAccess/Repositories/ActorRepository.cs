using Microsoft.EntityFrameworkCore;
using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

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
                    .ThenInclude(m => m.Genre)
                .Include(a => a.Movies)
                    .ThenInclude(m => m.Actors)
                .ToList();
        }

        public override Actor? GetById(int id)
        {
            return _dbSet
                .Include(a => a.Biography)
                .Include(a => a.Movies)
                    .ThenInclude(m => m.Genre)
                .Include(a => a.Movies)
                    .ThenInclude(m => m.Actors)
                .FirstOrDefault(a => a.Id == id);
        }

        public override IEnumerable<Actor> Find(Expression<Func<Actor, bool>> expression)
        {
            return _dbSet
                .Include(a => a.Biography)
                .Include(a => a.Movies)
                    .ThenInclude(m => m.Genre)
                .Include(a => a.Movies)
                    .ThenInclude(m => m.Actors)
                .Where(expression)
                .ToList();
        }
    }
}
