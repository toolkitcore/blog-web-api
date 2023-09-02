using Api.Core.Entities;
using Api.Infrastructure.Interface;
using Api.Infrastructure.Persistence;

namespace Api.ApplicationLogic.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}