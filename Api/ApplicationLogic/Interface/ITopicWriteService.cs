using Models.Topic;

namespace Api.ApplicationLogic.Interface
{
    public interface ITopicWriteService
    {
        Task<int> Add(TopicCreateModel request);
        Task<TopicDTO> Update(TopicUpdateModel request);
        Task<int> Delete(int id);
    }
}