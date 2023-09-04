using Api.Core.Entities;
using Api.Infrastructure.Interface;
using Api.Infrastructure.Persistence;

namespace Api.ApplicationLogic.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}