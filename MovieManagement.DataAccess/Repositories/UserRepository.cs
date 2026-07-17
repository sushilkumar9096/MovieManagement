using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;
using System.Linq;

namespace MovieManagement.DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public User? GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());
        }
    }
}
