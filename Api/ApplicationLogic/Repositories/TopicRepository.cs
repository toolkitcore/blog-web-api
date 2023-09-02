using Api.Core.Entities;
using Api.Infrastructure.Interface;
using Api.Infrastructure.Persistence;

namespace Api.ApplicationLogic.Repositories
{
    public class TopicRepository : GenericRepository<Topic>, ITopicRepository
    {
        public TopicRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}