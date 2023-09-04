namespace Api.Core.Entities
{
#nullable disable
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTimeOffset CommentDate { get; set; }
        public int CommenterID { get; set; }
        public User Commenter { get; set; }
        public int PostID { get; set; }
        public Blog Post { get; set; }
    }
}