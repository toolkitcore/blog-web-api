using Api.Core.Commons;
using Api.Core.Entities;

namespace Api.ApplicationLogic.Interface
{
    public interface ITopicReadService
    {
        Task<Pagination<Topic>> Get(int pageIndex, int pageSize);
        Task<Topic> Get(int id);
    }
}