using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;
using System;

namespace MovieManagement.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IActorRepository Actors { get; }
        public IMovieRepository Movies { get; }
        public IGenreRepository Genres { get; }
        public IBiographyRepository Biographies { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Actors = new ActorRepository(_context);
            Movies = new MovieRepository(_context);
            Genres = new GenreRepository(_context);
            Biographies = new BiographyRepository(_context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
