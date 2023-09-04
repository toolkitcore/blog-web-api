namespace Models.Blog
{
    public record BlogCreateModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // remove later
        public string Content { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public DateTimeOffset Date { get; set; }
        public ICollection<BlogTopicCreateModel> BlogTopics { get; set; }
    }
    public record BlogTopicCreateModel
    {
        public int BlogId { get; set; }
        public int TopicId { get; set; }
    }
}
