using MovieManagement.Domain.Entities;

namespace MovieManagement.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User? GetByUsername(string username);
    }
}
