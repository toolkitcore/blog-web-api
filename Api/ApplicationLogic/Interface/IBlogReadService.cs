using Api.Core.Commons;
using Api.Core.Entities;

namespace Api.ApplicationLogic.Interface
{
    public interface IBlogReadService
    {
        Task<Pagination<Blog>> Get(int pageIndex, int pageSize);
        Task<Blog> Get(int id);
    }
}