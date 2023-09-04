using Api.Core.Entities;
using Models.Blog;

namespace Api.ApplicationLogic.Interface
{
    public interface IBlogWriteService
    {
        Task<int> Add(BlogCreateModel request);
        Task<BlogCreateModel> Update(BlogUpdateModel request);
        Task<int> Delete(int id);
    }
}