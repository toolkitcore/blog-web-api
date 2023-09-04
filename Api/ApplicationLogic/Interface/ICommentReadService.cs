using Api.Core.Commons;
using Api.Core.Entities;

namespace Api.ApplicationLogic.Interface
{
    public interface ICommentReadService
    {
        Task<Pagination<Comment>> Get(int pageIndex, int pageSize);
        Task<Comment> Get(int id);
    }
}