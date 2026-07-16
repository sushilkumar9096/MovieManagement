using System;

namespace MovieManagement.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IActorRepository Actors { get; }
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        IBiographyRepository Biographies { get; }
        int Save();
    }
}
