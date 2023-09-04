using Models.Comment;

namespace Api.ApplicationLogic.Interface
{
    public interface ICommentWriteService
    {
        Task<int> Add(CommentCreateModel request);
        Task<CommentDTO> Update(CommentUpdateModel request);
        Task<int> Delete(int id);
    }
}