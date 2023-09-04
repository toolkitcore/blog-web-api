using Api.Core.Commons;
using Api.Core.Entities;
using Models.Topic;

namespace Api.ApplicationLogic.Interface
{
    public interface ITopicReadService
    {
        Task<Pagination<Topic>> Get(int pageIndex, int pageSize);
        Task<Topic> Get(int id);
    }
}