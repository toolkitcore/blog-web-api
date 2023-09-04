namespace Api.Core.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<BlogTopic> BlogTopics { get; set; }
    }
}