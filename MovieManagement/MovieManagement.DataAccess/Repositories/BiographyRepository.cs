using MovieManagement.Domain.Entities;
using MovieManagement.Domain.Repositories;
using MovieManagement.DataAccess.Data;

namespace MovieManagement.DataAccess.Repositories
{
    public class BiographyRepository : GenericRepository<Biography>, IBiographyRepository
    {
        public BiographyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
