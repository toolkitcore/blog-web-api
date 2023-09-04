namespace Api.Core.Entities
{
#nullable disable
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Topic> Topics { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<BlogTopic> BlogTopics { get; set; }
    }
}